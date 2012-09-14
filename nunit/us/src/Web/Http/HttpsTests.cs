using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.Log;
using OpenADK.Util;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using NUnit.Framework;
using Org.Mentalis.Security.Certificates;
using StoreLocation=Org.Mentalis.Security.Certificates.StoreLocation;
using System.Collections.Generic;
using Library.UnitTesting.Framework;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Tests HTTPS support in the ADK
    /// </summary>
    [TestFixture]
    public class HttpsTests
    {
        private HttpTransport fTransport = null;
        private SimpleHandler fHandler = null;
        private HttpsProperties fProps = null;
        private IZone fZone = null;
        private Agent fAgent = new TestAgent();

        private const string HANDLER_URL = "/";
        private const string SERVER_TEST_URL = "https://localhost:9000/";
        private const string CERT_STORE = CertificateStore.MyStore;

        /// <summary>
        /// This method runs once at the start of this fixture
        /// </summary>
        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            CertificateStore store = new CertificateStore(StoreLocation.LocalMachine, CertificateStore.RootStore);
            AssertTestCertFile(store, "issuer.pfx");

            store = new CertificateStore(StoreLocation.CurrentUser, CERT_STORE);
            AssertTestCertFile(store, "localhost.pfx");
            AssertTestCertFile(store, "127.0.0.1.pfx");
            AssertTestCertFile(store, "invalid.pfx");

            ConsoleAppender cAppender = new ConsoleAppender();
            cAppender.Layout = new PatternLayout(Adk.DEFAULT_LOG_PATTERN);
            SetLogAppender(cAppender, Level.Debug, false);
        }

        /// <summary>
        /// This method runs once the beginning of each test
        /// </summary>
        [SetUp]
        public void SetUpTest()
        {
            Adk.Debug = AdkDebugFlags.All;
            Adk.Initialize();

            ServicePointManager.CertificatePolicy = new TestCertificatePolicy();
            ServicePointManager.CheckCertificateRevocationList = false;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            fTransport = (HttpTransport)fAgent.TransportManager.GetTransport( "https" );

            fProps = (HttpsProperties) fTransport.Properties;
            fProps.Port = 9000;
            fProps.CertStore = CERT_STORE;
            fProps.ClientAuthLevel = 0;
            fProps.ClientCertName = null;
           


            fZone = new TestZone();
            fZone.Properties.MessagingMode = AgentMessagingMode.Push;
        }

        /// <summary>
        /// This method runs once at the end of every test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (fTransport != null)
            {
                fTransport.Shutdown();
                fTransport = null;
            }
        }

        private void AssertTestCertFile(CertificateStore store, string fileName)
        {
            FileInfo f = new FileInfo( fileName );
            if( f.Exists )
            {
                Console.WriteLine( f.FullName );
            }


            if (!File.Exists(fileName))
            {
                using (Stream stream = GetType().Assembly.GetManifestResourceStream("Library.Nunit.US.res." + fileName)
                    )
                {
                    using (Stream certFile = File.OpenWrite(fileName))
                    {
                        Streams.CopyStream(stream, certFile);
                        certFile.Close();
                    }
                    stream.Close();
                }
                Certificate cert = Certificate.CreateFromPfxFile(fileName, "changeit", true);

                Certificate found = store.FindCertificateByKeyIdentifier(cert.GetKeyIdentifier());
                if (found == null)
                {
                    store.AddCertificate(cert);
                }
            }
        }


        /// <summary>
        /// This test should set up an HTTPS server that requires
        /// client certificates to be sent. It should then try to 
        /// connect to the server without using client certs. 
        /// This should fail. It then should try with client
        /// certs enabled and it should succeed.
        /// </summary>
        /// <remarks>In order for this test to pass, the certificate used for
        /// client authentication must be trusted by the operating system.</remarks>
        [Test]
        public void TestLevel2AuthSupport()
        {
            fProps.ClientAuthLevel = 2;
            fProps.ClientCertName = "OU=ADK,CN=invalid";
            startupTransport();
    
            //RunConnectionTest( false, false );
            RunConnectionTest(true, true);
        }

        /// <summary>
        /// This test should set up an HTTPS server that requires
        /// client certificates to be sent. It should then try to 
        /// connect to the server using a valid certificate, but with a
        /// subject that does not match the host name. This should fail.
        /// </summary>
        /// <remarks>In order for this test to pass, the certificate used for
        /// client authentication must be trusted by the operating system.</remarks>
        [Test]
        public void TestLevel3AuthSupportWithInvalidHost()
        {
            fProps.ClientAuthLevel = 3;
            fProps.ClientCertName = "OU=ADK,CN=invalid";
            startupTransport();
         
            // Run the test with a client certificate, and it should fail
            RunConnectionTest(true, false);
        }


        [Test]
        public void TestLevel3AuthSupportWithValidHost()
        {
            fProps.ClientAuthLevel = 3;
            fProps.ClientCertName = "OU=ADK,CN=localhost";
            startupTransport();

            // Run the test with a client certificate, and it should not fail
            RunConnectionTest(true, true);
        }

        [Test]
        public void TestLevel3AuthSupportWithValidIP()
        {
            fProps.ClientAuthLevel = 3;
            fProps.ClientCertName = "OU=ADK,CN=127.0.0.1";
            startupTransport();
         
            // Run the test with a client certificate, and it should not fail
            RunConnectionTest(true, true);
        }

        [Test]
        public void TestHttps()
        {
            startupTransport();
         
            RunConnectionTest(false, true);
        }

        [Test]
        public void TestHttpsWithName()
        {
            fProps.SSLCertName = "OU=ADK,CN=localhost";
            startupTransport();

            RunConnectionTest(false, true);
            RunConnectionTest(true, true);
        }

        [Test]
        public void TestHttpsWithFile()
        {
            fProps.SSLCertFile = "localhost.pfx";
            fProps.SSLCertFilePassword = "changeit";
            startupTransport();
            RunConnectionTest(false, true);
        }

        private void startupTransport()
        {
            fTransport.Activate(fAgent);
            if (!fTransport.IsActive(fZone))
            {
                fTransport.Activate( fZone );
                fHandler = new SimpleHandler();
                ((AdkHttpApplicationServer) fTransport.Server).AddHandlerContext( "", HANDLER_URL, fHandler, true );
            }
        }


        private void RunConnectionTest(bool useClientCert, bool shouldPass)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(SERVER_TEST_URL);
            if (useClientCert)
            {
                Certificate cert = fTransport.GetClientAuthenticationCertificate();
                X509Certificate cert2 = cert.ToX509();
                request.ClientCertificates.Add(cert2);
            }

            bool sslConnectError = false;
            string errorMessage = string.Empty;
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                fHandler.AssertResponse(response);
            }
            catch (WebException wex)
            {
                if(wex.Status == WebExceptionStatus.SecureChannelFailure || wex.Status == WebExceptionStatus.SendFailure )
                {
                    sslConnectError = true;
                    errorMessage = wex.Message;
                }
                else
                {
                    throw;
                }
            }

            if (shouldPass)
            {
                Assert.IsFalse(sslConnectError, "Received an http exception: " + errorMessage);
            }
            else
            {
                Assert.IsTrue(sslConnectError, "Should have received an exception");
            }
        }


        private static void SetLogAppender(IAppender appender, Level level, bool additive)
        {
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            if (hierarchy != null)
            {
                if (!additive)
                {
                    hierarchy.Root.RemoveAllAppenders();
                }
                hierarchy.Root.AddAppender(appender);
                hierarchy.Threshold = level;
                hierarchy.Configured = true;
            }
            else
            {
                throw new AdkException("Unable to initialize log4net framework", null);
            }
        }


        private class SimpleHandler : IAdkHttpHandler
        {
            private const string OK_VALUE = "OK";
            private const string OK_HEADER = "CUSTOM_HEADER";

            public void ProcessRequest(AdkHttpRequestContext context)
            {
                context.Response.Headers.Add(OK_HEADER, OK_VALUE);
                context.Response.Write("OK");
            }

            public void AssertResponse(HttpWebResponse response)
            {
                // Write out the received headers for debugging purposes
                foreach (string key in response.Headers.AllKeys)
                {
                    Console.WriteLine("Header:{0} = \"{1}\"", key, response.Headers[key]);
                }

                Assert.AreEqual(OK_VALUE, response.Headers[OK_HEADER], "Did not receive proper response header");

                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string responseValue = reader.ReadToEnd();
                    Assert.AreEqual(OK_VALUE, responseValue);
                }
            }
        }

        private class TestCertificatePolicy : ICertificatePolicy
        {
            #region ICertificatePolicy Members

            public bool CheckValidationResult(
                ServicePoint srvPoint,
                X509Certificate certificate,
                WebRequest request,
                int certificateProblem)
            {
                // The .Net ADK uses the same validation as the default .Net framework certificate policy. If other
                // policy requirements become necessary for the SIF Specification or special situations, they can be
                // implemented here.
                if (certificateProblem == 0)
                {
                    return true;
                }
                return false;
            }

            #endregion
        }

        private class TestZone : IZone
        {
            private AgentProperties fProperties;

            #region IZone Members

            public SIF_Ack SifUnregister()
            {
                // TODO:  Add TestZone.SifUnregister implementation
                return null;
            }

            public void PurgeQueue(bool incoming, bool outgoing)
            {
                // TODO:  Add TestZone.PurgeQueue implementation
            }

            public SIF_Ack SifRegister()
            {
                // TODO:  Add TestZone.SifRegister implementation
                return null;
            }

            public void ReportEvent(SifDataObject obj, EventAction action, string destinationId)
            {
                // TODO:  Add TestZone.ReportEvent implementation
            }

            void IZone.ReportEvent(SifDataObject obj, EventAction action)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.ReportEvent implementation
            }

            void IZone.ReportEvent(Event ev)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.ReportEvent implementation
            }

            public SIF_Ack SifUnprovide(string[] objectType)
            {
                // TODO:  Add TestZone.SifUnprovide implementation
                return null;
            }

            public SIF_Ack SifPing()
            {
                // TODO:  Add TestZone.SifPing implementation
                return null;
            }

            public SIF_Ack SifSubscribe(string[] objectType)
            {
                // TODO:  Add TestZone.SifSubscribe implementation
                return null;
            }

            public string ZoneId
            {
                get
                {
                    // TODO:  Add TestZone.ZoneId getter implementation
                    return null;
                }
            }

            public void Disconnect(ProvisioningFlags flags)
            {
                // TODO:  Add TestZone.Disconnect implementation
            }

            public ServerLog ServerLog
            {
                get
                {
                    // TODO:  Add TestZone.ServerLog getter implementation
                    return null;
                }
            }

            public IProtocolHandler ProtocolHandler
            {
                get { return null; }
            }

            public IList<SifException> ConnectWarnings
            {
                get
                {
                    // TODO:  Add TestZone.ConnectWarnings getter implementation
                    return null;
                }
            }

            public void WakeUp()
            {
                // TODO:  Add TestZone.WakeUp implementation
            }

            /// <summary>  Determines if the agent's queue for this zone is in sleep mode.
            /// 
            /// </summary>
            /// <param name="flags">When AdkFlags.LOCAL_QUEUE is specified, returns true if the
            /// Agent Local Queue is currently in sleep mode. False is returned if
            /// the Agent Local Queue is disabled. When AdkFlags.SERVER_QUEUE is
            /// specified, queries the sleep mode of the Zone Integration Server
            /// by sending a SIF_Ping message.
            /// </param>
            public bool IsSleeping(AdkQueueLocation flags)
            {
                // TODO:  Add TestZone.IsSleeping implementation
                return false;
            }

            public bool Connected
            {
                get
                {
                    // TODO:  Add TestZone.Connected getter implementation
                    return false;
                }
            }

            public void SetPublisher(IPublisher publisher, IElementDef objectType, PublishingOptions options)
            {
                // TODO:  Add TestZone.SetPublisher implementation
            }


            void IProvisioner.SetPublisher(IPublisher publisher)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.SetPublisher implementation
            }

            /// <summary>
            /// Register a Publisher message handler with this zone to process SIF_Requests
            /// for the specified object type. This method may be called repeatedly for
            /// each SIF Data Object type the agent will publish on this zone.
            /// </summary>
            /// <param name="publisher">
            /// An object that implements the <code>Publisher</code>
            /// interface to respond to SIF_Request queries received by the agent,
            /// where the SIF object type referenced by the request matches the
            /// specified objectType. This Publisher will be called whenever a
            /// SIF_Request is received on this zone and no other object in the
            /// message dispatching chain has processed the message.
            /// </param>
            /// <param name="objectType">An ElementDef constant from the SIFDTD class that 
            /// identifies a SIF Data Object type. E.g. SIFDTD.STUDENTPERSONAL
            /// </param>
            public void SetPublisher(IPublisher publisher, IElementDef objectType)
            {
                throw new NotImplementedException();
            }

            public void RemoveMessagingListener(IMessagingListener listener)
            {
                // TODO:  Add TestZone.RemoveMessagingListener implementation
            }

           public void SetSubscriber(ISubscriber subscriber, IElementDef objectType, SubscriptionOptions flags)
            {
                // TODO:  Add TestZone.SetSubscriber implementation
            }

            public SIF_Ack SifUnsubscribe(string[] objectType)
            {
                // TODO:  Add TestZone.SifUnsubscribe implementation
                return null;
            }


            public IUndeliverableMessageHandler ErrorHandler
            {
                get
                {
                    // TODO:  Add TestZone.ErrorHandler getter implementation
                    return null;
                }
                set
                {
                    // TODO:  Add TestZone.ErrorHandler setter implementation
                }
            }

            public Uri ZoneUrl
            {
                get
                {
                    // TODO:  Add TestZone.ZoneUrl getter implementation
                    return null;
                }
            }

            public ILog Log
            {
                get
                {
                    // TODO:  Add TestZone.Log getter implementation
                    return null;
                }
            }

            public SIF_ZoneStatus GetZoneStatus(TimeSpan timeout)
            {
                // TODO:  Add TestZone.GetZoneStatus implementation
                return null;
            }

            SIF_ZoneStatus IZone.GetZoneStatus()
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.GetZoneStatus implementation
                return null;
            }

            public void AddMessagingListener(IMessagingListener listener)
            {
                // TODO:  Add TestZone.AddMessagingListener implementation
            }

            ///<summary>
            ///Register a Subscriber message handler with this zone to process SIF_Event
            /// messages for the specified object type. This method may be called 
            /// repeatedly for each SIF Data Object type the agent subscribes to on 
            /// this zone.
            /// </summary>
            /// <param name="subscriber">
            /// An object that implements the <code>Subscriber</code>
            /// interface to respond to SIF_Event notifications received by the agent,
            /// where the SIF object type referenced by the request matches the
            /// specified objectType. This Subscriber will be called whenever a
            /// SIF_Event is received on this zone and no other object in the
            /// message dispatching chain has processed the message.
            /// </param>
            /// <param name="objectType">
            /// A constant from the SIFDTD class that identifies a
            /// SIF Data Object type.
            ///</param>
            public void SetSubscriber(ISubscriber subscriber, IElementDef objectType)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                // TODO:  Add TestZone.ToString implementation
                return null;
            }

            public Agent Agent
            {
                get
                {
                    // TODO:  Add TestZone.Agent getter implementation
                    return null;
                }
            }

            public void Connect(ProvisioningFlags provOptions)
            {
                // TODO:  Add TestZone.Connect implementation
            }

            public AgentProperties Properties
            {
                get
                {
                    if( fProperties == null )
                    {
                        fProperties = new AgentProperties( null );
                    }
                    return fProperties;
                }
                set
                {
                    fProperties = value;
                }
            }

            public SIF_Ack SifSend(string xml)
            {
                // TODO:  Add TestZone.SifSend implementation
                return null;
            }

           public string Query(Query query, IMessagingListener listener, string destinationId,
                                AdkQueryOptions queryOptions)
            {
                // TODO:  Add TestZone.Query implementation
                return null;
            }

           string IZone.Query(Query query, string destinationId, AdkQueryOptions queryOptions)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.Query implementation
                return null;
            }

           string IZone.Query(Query query, IMessagingListener listener, AdkQueryOptions queryOptions)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.Query implementation
                return null;
            }

           string IZone.Query(Query query, AdkQueryOptions queryOptions)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.Query implementation
                return null;
            }

           string IZone.Query(Query query, IMessagingListener listener)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.Query implementation
                return null;
            }

           string IZone.Query(Query query)
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.Query implementation
                return null;
            }

            public object UserData
            {
                get
                {
                    // TODO:  Add TestZone.UserData getter implementation
                    return null;
                }
                set
                {
                    // TODO:  Add TestZone.UserData setter implementation
                }
            }

            public SIF_Ack SifProvide(string[] objectType)
            {
                // TODO:  Add TestZone.SifProvide implementation
                return null;
            }

            void IProvisioner.SetQueryResults(IQueryResults queryResults, IElementDef objectType, QueryResultsOptions flags)
            {
                // TODO:  Add TestZone.SetQueryResults implementation
            }

            void IProvisioner.SetQueryResults(IQueryResults queryResults )
            {
                // TODO:  Add TestZone.OpenADK.Library.IZone.SetQueryResults implementation
            }

            #region IProvisioner Members

            /// <summary>
            /// Register a QueryResults object with this zone for the specified SIF object type.
            /// </summary>
            /// <param name="queryResults">
            /// An object that implements the <code>QueryResults</code>
            /// interface to respond to SIF_Response query results received by the agent,
            /// where the SIF object type referenced by the request matches the
            /// specified objectType. This QueryResults object will be called whenever
            /// a SIF_Response is received on this zone and no other object in the
            /// message dispatching chain has processed the message.
            ///</param>
            ///<param name="objectType">
            /// A constant from the SIFDTD class that identifies a
            /// SIF Data Object type.
            /// </param>
            public void SetQueryResults(IQueryResults queryResults, IElementDef objectType)
            {
                throw new NotImplementedException();
            }

            #endregion

            public void Sleep()
            {
                // TODO:  Add TestZone.Sleep implementation
            }

            #endregion
        }
    }
}