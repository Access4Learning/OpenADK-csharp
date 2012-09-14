//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Security.Cryptography;
using System.Text;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>
    /// SifEncryption provides easy read and write access to encrypted passwords in the Authentication (US) or Identity(UK) objects
    /// </summary>
    /// <remarks>
    /// <para>
    /// The SIFEncryption class uses the following properties to determine its default algorithm and key for writing and for finding keys for decrypting passwords. 
    /// Please see <see cref="AgentProperties"/> for details
    /// </para>
    /// <list type="table">
    /// <listheader><term>Settings Key</term><description>Description</description></listheader>
    /// <item><term>adk.encryption.algorithm</term><description>The default algorithm used for writing passwords</description></item>
    /// <item><term>adk.encryption.key</term><description>The name of the default key to use for encryption</description></item>
    /// <item><term>adk.encryption.keys.[keyname]</term><description>The actual key to use for encryption or decryption where “keyname” matches the @KeyName attribute of the Password object</description></item>
    /// </list>
    /// 
    /// </remarks>
    /// <example>Here is an example of how a subscriber to the US Authentication object could use SifEncryption
    /// to read passwords. For more examples, including a provider example, please see the AuthenticationProvider and AuthenticationSubscriber
    /// example projects.
    /// <code lang="VB.Net">
    /// Public Sub OnQueryResults(ByVal data As OpenADK.Library.IDataObjectInputStream, _
    ///     ByVal err As OpenADK.Library.Infra.SIF_Error, _
    ///     ByVal zone As OpenADK.Library.IZone, _
    ///     ByVal info As OpenADK.Library.IMessageInfo) _
    ///     Implements OpenADK.Library.IQueryResults.OnQueryResults
    ///     While data.Available
    ///         Dim auth As Authentication = DirectCast(data.ReadDataObject(), Authentication)
    ///         HandleAuthenticationReceived(auth, zone)
    ///     End While
    /// End Sub
    /// 
    /// Public Sub OnEvent(ByVal evnt As OpenADK.Library.Event, _
    ///     ByVal zone As OpenADK.Library.IZone, _
    ///     ByVal info As OpenADK.Library.IMessageInfo) _
    ///     Implements OpenADK.Library.ISubscriber.OnEvent
    ///     While evnt.Data.Available
    ///         Dim auth As Authentication = DirectCast(evnt.Data.ReadDataObject(), Authentication)
    ///         HandleAuthenticationReceived(auth, zone)
    ///     End While
    /// End Sub
    /// 
    /// Private Sub HandleAuthenticationReceived(ByVal auth As Authentication, ByVal zone As IZone)
    ///     Dim inf As AuthenticationInfo
    ///     For Each inf In auth.GetAuthenticationInfos()
    ///         Dim decryptor As SifEncryption = SifEncryption.GetInstance(inf.Password, zone)
    ///         Console.WriteLine("Received AuthenticationInfo/Password using algorithm {0} for user {1}, password={2} ", _
    ///         decryptor.Algorithm.ToString(), inf.Username, decryptor.ReadPassword(inf.Password))
    ///         Console.WriteLine()
    ///     Next
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// public void OnQueryResults(OpenADK.Library.IDataObjectInputStream data, 
    /// 	OpenADK.Library.Infra.SIF_Error err, 
    /// 	OpenADK.Library.IZone zone, 
    /// 	OpenADK.Library.IMessageInfo info)
    /// {
    /// 	while (data.Available) 
    /// 	{
    /// 		Authentication auth = ((Authentication)(data.ReadDataObject()));
    /// 		HandleAuthenticationReceived(auth, zone);
    /// 	}
    /// }
    /// 
    /// public void OnEvent(OpenADK.Library.Event evnt, 
    /// 	OpenADK.Library.IZone zone, 
    /// 	OpenADK.Library.IMessageInfo info)
    /// {
    /// 	while (evnt.Data.Available) 
    /// 	{
    /// 		Authentication auth = ((Authentication)(evnt.Data.ReadDataObject()));
    /// 		HandleAuthenticationReceived(auth, zone);
    /// 	}
    /// }
    /// 
    /// private void HandleAuthenticationReceived(Authentication auth, IZone zone)
    /// {
    /// 	foreach (AuthenticationInfo inf in auth.GetAuthenticationInfos()) 
    /// 	{
    /// 		SifEncryption decryptor = SifEncryption.GetInstance(inf.Password, zone);
    /// 		Console.WriteLine("Received AuthenticationInfo/Password using algorithm {0} for user {1}, password={2} ", 
    /// 			decryptor.Algorithm.ToString(), inf.Username, decryptor.ReadPassword(inf.Password));
    /// 		Console.WriteLine();
    /// 	}
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="AgentProperties"/>
    public abstract class SifEncryption : IDisposable
    {
        private PasswordAlgorithm fAlgorithm;
        private string fkeyName;
        private static SifEncryption sCurrentInstance;
        private bool fDisposed;

        private SifEncryption( PasswordAlgorithm algorithm,
                               string keyName )
        {
            fkeyName = keyName;
            fAlgorithm = algorithm;
        }


        /// <summary>
        /// Creates an instance of SIFEncryption that uses the specified 
        /// PasswordAlgorithm, keyName and key
        /// </summary>
        /// <param name="algorithm">The algorithm to use for encrypting or decrypting passwords</param>
        /// <param name="keyName">The name of the encryption key to use.</param>
        /// <param name="key">The encryption key to use. This parameter is ignored for
        /// SHA1 and MD5 because they are not keyed hash algorithms </param>
        /// <returns>An instance of the SifEncryption class for reading and writing passwords</returns>
        public static SifEncryption GetInstance(
            PasswordAlgorithm algorithm,
            string keyName,
            byte [] key
            )
        {
            if ( sCurrentInstance != null ) {
                if ( !sCurrentInstance.fDisposed &&
                     sCurrentInstance.Algorithm.Value.Equals( algorithm.Value ) &&
                     (sCurrentInstance.KeyName == keyName || sCurrentInstance.Key == null) ) {
                    return sCurrentInstance;
                }
                else {
                    sCurrentInstance.Dispose();
                    sCurrentInstance = null;
                }
            }

            if (algorithm.ValueEquals("base64")){
                sCurrentInstance = new SifClearTextEncryption( algorithm, keyName );
            }
            else if ( algorithm.Value == PasswordAlgorithm.SHA1.Value ) {
                sCurrentInstance = new SifHashEncryption( algorithm, keyName, new SHA1Managed() );
            }
            else if ( algorithm.Value == PasswordAlgorithm.MD5.Value ) {
                sCurrentInstance =
                    new SifHashEncryption( algorithm, keyName, new MD5CryptoServiceProvider() );
            }
            else if ( algorithm.Value == PasswordAlgorithm.DES.Value ) {
                sCurrentInstance =
                    new SifSymmetricEncryption
                        ( algorithm, keyName, new DESCryptoServiceProvider(), key );
            }
            else if ( algorithm.Value == PasswordAlgorithm.TRIPLEDES.Value ) {
                sCurrentInstance =
                    new SifSymmetricEncryption
                        ( algorithm, keyName, new TripleDESCryptoServiceProvider(), key );
            }
            else if ( algorithm.Value == PasswordAlgorithm.RC2.Value ) {
                sCurrentInstance =
                    new SifSymmetricEncryption
                        ( algorithm, keyName, new RC2CryptoServiceProvider(), key );
            }
            else {
                throw new AdkNotSupportedException
                    ( string.Format( "Encryption algorithm {0} is not supported.", algorithm.Value ) );
            }

            return sCurrentInstance;
        }


        /// <summary>
        /// Creates an instance of SIFEncryption that can decrypt
        /// the password field automatically, using settings 
        /// defined in the agent's properties.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method searches the agent properties in effect 
        /// for the zone and looks for one that matches the key 
        /// defined in the Password object.
        /// If it finds one, it returns an instance of SIFEncryption
        /// that has been initialized with the proper key and 
        /// encryption algorithm for the field.
        /// </para>
        /// <para>
        /// This method looks for a property named 
        /// "adk.encryption.keys.[key]" where [key] is the name
        /// of the key field defined by the Password
        /// field.
        /// </para>
        /// </remarks>
        /// <param name="password">The password object that needs
        /// to be decrypted</param>
        /// <param name="zone">The zone that is in scope for the 
        /// current message</param>
        /// <returns></returns>
        public static SifEncryption GetInstance(
            Password password,
            IZone zone )
        {
            if ( sCurrentInstance != null &&
                 sCurrentInstance.Algorithm.Value == password.Algorithm &&
                 (sCurrentInstance.KeyName == password.KeyName || sCurrentInstance.Key == null) ) {
                return sCurrentInstance;
            }
            byte [] key = zone.Properties.GetEncryptionKey( password.KeyName );
            return
                GetInstance( PasswordAlgorithm.Wrap( password.Algorithm ), password.KeyName, key );
        }

        /// <summary>
        /// Creates an instance of SIFEncryption that can be used
        /// for writing the Password field,
        /// using settings from the agent's properties
        /// </summary>
        /// <remarks>
        /// This method searches for two properties in the agent
        /// properties. "adk.encryption.algorithm" returns the default
        /// algorithm the agent uses for encryption. 
        /// "adk.encryption.key" returns the name of the key to use for
        /// encryption, which the agent then retrieves from the
        /// "adk.encryption.keys.[key]" property.
        /// </remarks>
        /// <returns>An instance of the SIfEncryption class</returns>
        public static SifEncryption GetInstance( IZone zone )
        {
            string alg = zone.Properties.DefaultEncryptionAlgorithm;
            string keyName = zone.Properties.DefaultEncryptionKeyName;
            if ( alg == null ) {
                throw new AdkException
                    ( "The default encryption algorithm or default key name is not defined in the agent or zone properties",
                      zone );
            }
            byte [] key = null;
            if ( keyName != null ) {
                zone.Properties.GetEncryptionKey( keyName );
            }
            // Don't check for a null key at this time because some of the algorithms don't even need a key
            return GetInstance( PasswordAlgorithm.Wrap( alg ), keyName, key );
        }


        /// <summary>
        /// Encrypts the specified password and populates the 
        /// Password field with the algorithm and key name
        /// values. This method must be overriden by the specific encryption
        /// algorithm to set the actual encrypted value.
        /// </summary>
        /// <param name="password">The password object to populate</param>
        /// <param name="value">The value to encryp</param>
        public virtual void WritePassword( Password password,
                                           string value )
        {
            password.SetAlgorithm( fAlgorithm );
            password.KeyName = fkeyName;
        }

        /// <summary>
        /// Returns the unencrypted password value from the 
        /// Password field. If the algorithm in use is a hash
        /// algorithm, the Base64 instance of the hash will be returned instead.
        /// </summary>
        /// <param name="password">The field that needs to be decrypted</param>
        /// <returns>The unencrypted password</returns>
        public abstract string ReadPassword( Password password );

        /// <summary>
        /// Cleans up any unmanaged resources that are being held on to by this instance
        /// </summary>
        public virtual void Dispose()
        {
            fDisposed = true;
        }


        /// <summary>
        /// Returns the algorithm that is currently being used
        /// </summary>
        public PasswordAlgorithm Algorithm
        {
            get { return fAlgorithm; }
        }

        /// <summary>
        /// Returns the name of the key that is currently being used
        /// </summary>
        public string KeyName
        {
            get { return fkeyName; }
        }


        /// <summary>
        /// Returns the key that the class is currently using to decrypt
        /// </summary>
        public abstract byte [] Key { get; }

        /// <summary>
        /// Returns true if the value is a hashed value and cannot be decrypted. In this case,
        /// the <see cref="ReadPassword"/> method will return the hashed value as a Base64 string
        /// </summary>
        public abstract bool IsHash { get; }


        private class SifSymmetricEncryption : SifEncryption
        {
            private SymmetricAlgorithm fSymmetricAlgorithm;

            public SifSymmetricEncryption( PasswordAlgorithm algorithm,
                                           string keyName,
                                           SymmetricAlgorithm alg,
                                           byte [] key )
                : base( algorithm, keyName )
            {
                fSymmetricAlgorithm = alg;
                alg.Key = key;
                alg.BlockSize = 64;
                alg.Mode = CipherMode.CBC;
                alg.Padding = PaddingMode.PKCS7;
            }

            /// <summary>
            /// Encrypts the specified password and populates the 
            /// Password field with the necessary
            /// values
            /// </summary>
            /// <param name="password">The password object to populate</param>
            /// <param name="value">The value to encryp</param>
            public override void WritePassword( Password password,
                                                string value )
            {
                base.WritePassword( password, value );

                fSymmetricAlgorithm.GenerateIV();
                byte [] source = Encoding.UTF8.GetBytes( value );
                byte [] encryptedPassword;
                using ( ICryptoTransform encryptor = fSymmetricAlgorithm.CreateEncryptor() ) {
                    encryptedPassword = encryptor.TransformFinalBlock( source, 0, source.Length );
                }
                byte [] finalValue = new byte[8 + encryptedPassword.Length];
                Array.Copy( fSymmetricAlgorithm.IV, 0, finalValue, 0, 8 );
                Array.Copy( encryptedPassword, 0, finalValue, 8, encryptedPassword.Length );

                password.TextValue = Convert.ToBase64String( finalValue );
            }

            /// <summary>
            /// Returns the unencrypted password value from the 
            /// Password field
            /// </summary>
            /// <param name="password">The field that needs to be decrypted</param>
            /// <returns>The unencrypted password</returns>
            public override string ReadPassword( Password password )
            {
                byte [] encryptedValue = Convert.FromBase64String( password.TextValue );
                byte [] iv = new byte[8];
                Array.Copy( encryptedValue, 0, iv, 0, 8 );

                fSymmetricAlgorithm.IV = iv;
                byte [] decryptedPassword;
                using ( ICryptoTransform decryptor = fSymmetricAlgorithm.CreateDecryptor() ) {
                    decryptedPassword =
                        decryptor.TransformFinalBlock
                            ( encryptedValue, 8, encryptedValue.Length - 8 );
                }
                return Encoding.UTF8.GetString( decryptedPassword );
            }

            /// <summary>
            /// Returns the key that the class is currently using to decrypt
            /// </summary>
            public override byte [] Key
            {
                get { return fSymmetricAlgorithm.Key; }
            }

            /// <summary>
            /// Returns true if the value is a hashed value and cannot be decrypted. In this case,
            /// the <see cref="ReadPassword"/> method will return the hashed value as a Base64 string
            /// </summary>
            public override bool IsHash
            {
                get { return false; }
            }
        }

        private class SifClearTextEncryption : SifEncryption
        {
            public SifClearTextEncryption( PasswordAlgorithm algorithm,
                                           string keyName )
                : base( algorithm, string.Empty ) {}

            /// <summary>
            /// Encrypts the specified password and populates the 
            /// Password field with the necessary
            /// values
            /// </summary>
            /// <param name="password">The password object to populate</param>
            /// <param name="value">The value to encryp</param>
            public override void WritePassword( Password password,
                                                string value )
            {
                base.WritePassword( password, value );
                byte [] clearTextPassword = Encoding.UTF8.GetBytes( value );
                password.TextValue = Convert.ToBase64String( clearTextPassword );
            }

            /// <summary>
            /// Returns the unencrypted password value from the 
            /// Password field
            /// </summary>
            /// <param name="password">The field that needs to be decrypted</param>
            /// <returns>The unencrypted password</returns>
            public override string ReadPassword( Password password )
            {
                byte [] clearTextPassword = Convert.FromBase64String( password.TextValue );
                return Encoding.UTF8.GetString( clearTextPassword );
            }

            /// <summary>
            /// Returns the key that the class is currently using to decrypt. This implementation always returns null.
            /// </summary>
            public override byte [] Key
            {
                get { return null; }
            }

            /// <summary>
            /// Returns true if the value is a hashed value and cannot be decrypted. In this case,
            /// the <see cref="ReadPassword"/> method will return the hashed value as a Base64 string
            /// </summary>
            public override bool IsHash
            {
                get { return false; }
            }
        }

        private class SifHashEncryption : SifEncryption
        {
            private HashAlgorithm fHashAlgorithm;

            public SifHashEncryption( PasswordAlgorithm algorithm,
                                      string keyName,
                                      HashAlgorithm alg )
                : base( algorithm, string.Empty )
            {
                fHashAlgorithm = alg;
            }

            /// <summary>
            /// Encrypts the specified password and populates the 
            /// Password field with the necessary
            /// values
            /// </summary>
            /// <param name="password">The password object to populate</param>
            /// <param name="value">The value to encryp</param>
            public override void WritePassword( Password password,
                                                string value )
            {
                base.WritePassword( password, value );

                byte [] pass = Encoding.UTF8.GetBytes( value );
                byte [] hashedPassword = fHashAlgorithm.ComputeHash
                    (
                    pass );
                password.TextValue = Convert.ToBase64String( hashedPassword );
            }

            /// <summary>
            /// Returns the password value as a Base64 string
            /// </summary>
            /// <param name="password">The field that needs to be decrypted</param>
            /// <returns>The unencrypted password</returns>
            public override string ReadPassword( Password password )
            {
                return password.TextValue;
            }

            /// <summary>
            /// Returns the key that the class is currently using to decrypt. This implementation
            /// always returns null;
            /// </summary>
            public override byte [] Key
            {
                get { return null; }
            }

            /// <summary>
            /// Returns true if the value is a hashed value and cannot be decrypted. In this case,
            /// the <see cref="ReadPassword"/> method will return the hashed value as a Base64 string
            /// </summary>
            public override bool IsHash
            {
                get { return true; }
            }

            public override void Dispose()
            {
                base.Dispose();
                IDisposable hash = fHashAlgorithm as IDisposable;
                if ( hash != null ) {
                    hash.Dispose();
                }
            }
        }
    }
}
