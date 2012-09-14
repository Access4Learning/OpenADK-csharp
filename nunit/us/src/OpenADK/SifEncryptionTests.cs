using System;
using System.Security.Cryptography;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Infrastructure;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US
{
    /// <summary>
    /// Summary description for SifEncryptionTests.
    /// </summary>
    [TestFixture]
    public class SifEncryptionTests
    {
        private byte[] f64BitKey;
        private byte[] f128BitKey;
        private byte[] f192BitKey;
        private const string DEFAULT_ENCRYPTED_STRING = "¿sècrèt";

        [TestFixtureSetUp]
        public void SetUp()
        {
            Adk.Initialize();

            /* 
			f64BitKey = new byte[8];
			RNGCryptoServiceProvider.Create().GetBytes( f64BitKey );
			*/

            f64BitKey = Convert.FromBase64String("dW7SKzwdn0Q=");
            f128BitKey = Convert.FromBase64String("TcdilmUZ6qvbmegl2it2pA==");
            f192BitKey = Convert.FromBase64String("mECbXMo+fOMWRwam7tyUEE59jbO9O0Z4");

            StringBuilder builder = new StringBuilder();
            builder.Append("Created Unique 64-bit Encryption Key: ");
            foreach (byte b in f64BitKey)
            {
                builder.AppendFormat("0x{0:X}", b);
                builder.Append(new char[] {',', ' '});
            }
            builder.Append("\r\nBase 64 Value: ");
            builder.Append(Convert.ToBase64String(f64BitKey));
            Console.WriteLine(builder.ToString());

            /*
			f128BitKey = new byte[16];
			RNGCryptoServiceProvider.Create().GetBytes( f128BitKey );
			*/

            builder = new StringBuilder();
            builder.Append("Created Unique 128-bit Encryption Key: ");
            foreach (byte b in f128BitKey)
            {
                builder.AppendFormat("0x{0:X}", b);
                builder.Append(new char[] {',', ' '});
            }
            builder.Append("\r\nBase 64 Value: ");
            builder.Append(Convert.ToBase64String(f128BitKey));

            Console.WriteLine(builder.ToString());


            builder = new StringBuilder();
            builder.Append("Created Unique 192-bit Encryption Key: ");
            foreach (byte b in f192BitKey)
            {
                builder.AppendFormat("0x{0:X}", b);
                builder.Append(new char[] {',', ' '});
            }
            builder.Append("\r\nBase 64 Value: ");
            builder.Append(Convert.ToBase64String(f192BitKey));

            Console.WriteLine(builder.ToString());
        }

        private Authentication CreateAuthentication()
        {
            AuthenticationInfo inf =
                new AuthenticationInfo(new AuthSystem(AuthSystemType.APPLICATION, "Sample SIF Application"));
            inf.DistinguishedName = "cn=Example User, cn=Users, dc=sifinfo, dc=org";
            inf.Username = "example_user";
            Authentication auth = new Authentication(Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.STAFFPERSONAL);
            auth.AuthenticationInfo = inf;
            return auth;
        }

        /// <summary>
        /// Tests the SifEncryption Class using clear text encryption
        /// </summary>
        [Test]
        public void TestClearTextEncryption()
        {
            SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.BASE64, "Base64", null);
            Assert.AreEqual(string.Empty, encr.KeyName, "Encrytor should have an empty keyName");
            AuthenticationInfo info = AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
            Assert.AreEqual(string.Empty, info.PasswordList.ItemAt(0).KeyName,
                            "Password/@KeyName should have an empty value");
        }

        /// <summary>
        /// Tests the SifEncryption Class using DES encryption
        /// </summary>
        [Test]
        public void TestDESEncryption()
        {
            SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.DES, "SECRET_64_BIT_KEY", f64BitKey);
            AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
        }

        /// <summary>
        /// Tests the SifEncryption Class usingTripleDES encryption
        /// </summary>
        [Test]
        public void TestTripleDESEncryption()
        {
            SifEncryption encr =
                SifEncryption.GetInstance(PasswordAlgorithm.TRIPLEDES, "SECRET_192_BIT_KEY", f192BitKey);
            AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
        }

        /// <summary>
        /// Tests the SifEncryption Class using RC2 encryption
        /// </summary>
        [Test]
        public void TestRC2Encryption()
        {
            SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.RC2, "SECRET_128_BIT_KEY", f128BitKey);
            AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
        }

        /// <summary>
        /// Tests the SifEncryption Class using MD5 Hash
        /// </summary>
        [Test]
        public void TestMD5Hash()
        {
            SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.MD5, "MD5", null);
            Assert.AreEqual(string.Empty, encr.KeyName, "Encrytor should have an empty keyName");
            AuthenticationInfo info = AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
            Assert.AreEqual(string.Empty, info.PasswordList.ItemAt(0).KeyName,
                            "Password/@KeyName should have an empty value");
        }

        /// <summary>
        /// Tests the SifEncryption Class using SHA1 Hash
        /// </summary>
        [Test]
        public void TestSHA1Hash()
        {
            SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.SHA1, "SHA1", null);
            Assert.AreEqual(string.Empty, encr.KeyName, "Encrytor should have an empty keyName");
            AuthenticationInfo info = AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
            Assert.AreEqual(string.Empty, info.PasswordList.ItemAt(0).KeyName,
                            "Password/@KeyName should have an empty value");
        }

        /// <summary>
        /// Tests the SifEncryption Class using clear text encryption
        /// </summary>
        //[Test, Explicit]
        //public void TestRSAEncryption()
        //{
        //    // This test is not currently run with the full suite of tests because support for RSA encryption is
        //    // not implemented in the ADK
        //    SifEncryption encr = SifEncryption.GetInstance(PasswordAlgorithm.RSA, "SECRET_KEY_RSA", null);
        //    AssertEncryption(encr, DEFAULT_ENCRYPTED_STRING);
        //}

        /// <summary>
        /// Asserts that the password is encrypted and decrypted properly and returns the AuthenticationInfo
        /// object that was produced in test for further assertions, if necessary
        /// </summary>
        /// <param name="encryptor"></param>
        /// <param name="passwordText"></param>
        /// <returns></returns>
        private AuthenticationInfo AssertEncryption(SifEncryption encryptor, string passwordText)
        {
            AuthenticationInfo returnValue = null;

            Authentication auth = CreateAuthentication();
            AuthenticationInfo inf = auth.AuthenticationInfo;
            inf.PasswordList = new PasswordList();
            inf.PasswordList.Add(new Password());
            // Encrypt the password
            encryptor.WritePassword(inf.PasswordList.ItemAt(0), passwordText);

            // Write the object to and and read from xml to assure that the values are being persisted properly
            Authentication reparsedAuth =
                (Authentication) AdkObjectParseHelper.WriteParseAndReturn(auth, Adk.SifVersion);
            returnValue = reparsedAuth.AuthenticationInfo;

            SifEncryption decryptor =
                SifEncryption.GetInstance(PasswordAlgorithm.Wrap(returnValue.PasswordList.ItemAt(0).Algorithm),
                                          encryptor.KeyName, encryptor.Key);

            string decryptedValue = decryptor.ReadPassword(returnValue.PasswordList.ItemAt(0));
            if (encryptor.IsHash)
            {
                // Assert that the decrypted value is the same as the AuthenticationInfoPassword's text value
                Assert.AreEqual(returnValue.PasswordList.ItemAt(0).TextValue, decryptedValue,
                                "Hashed implementation of ReadPassword() should return the Base64 value");
                // Assert that the hash is correct
                HashAlgorithm hasher = null;
                if (returnValue.PasswordList.ItemAt(0).Algorithm == PasswordAlgorithm.SHA1.Value)
                {
                    hasher = new SHA1CryptoServiceProvider();
                }
                else if (returnValue.PasswordList.ItemAt(0).Algorithm == PasswordAlgorithm.MD5.Value)
                {
                    hasher = new MD5CryptoServiceProvider();
                }
                byte[] preHashed = Encoding.UTF8.GetBytes(passwordText);
                byte[] hashed = hasher.ComputeHash(preHashed);
                string textHash = Convert.ToBase64String(hashed);
                ((IDisposable) hasher).Dispose();

                Assert.AreEqual(textHash, decryptedValue, "Hash values do not match");
            }
            else
            {
                Assert.AreEqual(passwordText, decryptedValue, "Decypted value differs from original value.");
            }

            return returnValue;
        }
    }
}