using System;
using System.Runtime.Serialization;
using System.Threading;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Student;
using OpenADK.Util;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Impl
{
   /// <summary>
   /// Summary description for MessageDispatcherTests.
   /// </summary>
   [TestFixture]
   public class MessageDispatcherTests : InMemoryProtocolTest
   {
      //private Agent fAgent;
      //private ZoneImpl fZone;

      private static String MSG_GUID = "MESSAGE_DISPATCH_TESTS";

      //[SetUp]
      //public void setUp()
      //{
      //    Adk.Initialize();

      //    fAgent = new TestAgent();
      //    fAgent.Initialize();
      //    fZone = new TestZoneImpl("test", "http://127.0.0.1/test", fAgent, null); //:7080
      //}

      [Test]
      public void testNormalEvent()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler = new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.Normal);

         SubscriptionOptions options = new SubscriptionOptions();

         fZone.SetSubscriber(handler, objType, options);
         fZone.Connect(ProvisioningFlags.Register);
         //fZone.SetSubscriber(handler, objType, null);
         SIF_Event evnt = createSIF_Event(objType);
         assertNormalHandling(handler, evnt, fZone);
      }

      [Test]
      public void testEventThrowsNullPointerException()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowNullPointerException);
      
         SubscriptionOptions options = new SubscriptionOptions();

         fZone.SetSubscriber(handler, objType, options);
         fZone.Connect(ProvisioningFlags.Register);
         //

         //fZone.SetSubscriber(handler, objType, null);
         SIF_Event evnt = createSIF_Event(objType);
         AssertExceptionHandling(handler, evnt, fZone, typeof(NullReferenceException));
      }

      [Test]
      public void testEventThrowsException()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler = new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowException);
          
         SubscriptionOptions options = new SubscriptionOptions();

         fZone.SetSubscriber(handler, objType, options);
         fZone.Connect(ProvisioningFlags.Register);
         //

         // fZone.SetSubscriber(handler, objType, null);
         SIF_Event evnt = createSIF_Event(objType);
         AssertExceptionHandling(handler, evnt, fZone, typeof(AdkException));
      }

      [Test]
      public void testSIFRetryEvent()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler = new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowADKRetryException);

         fZone.SetSubscriber(handler, objType, null);
         SIF_Event evnt = createSIF_Event(objType);
         fZone.Connect(ProvisioningFlags.Register);
         AssertRetryHandling(handler, evnt, fZone);
      }

      [Test]
      public void testUndeliverableRetryEvent()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowSIFRetryException);
         fAgent.ErrorHandler = handler;
         SIF_Event evnt = createSIF_Event(objType);
         fZone.Connect(ProvisioningFlags.Register);
         AssertRetryHandling(handler, evnt, fZone);
      }

      private SIF_Event createSIF_Event(IElementDef objType)
      {
         SIF_Event evnt = new SIF_Event();
         evnt.Header.SIF_SourceId = "foo";
         SIF_ObjectData sod = new SIF_ObjectData();
         SIF_EventObject obj = new SIF_EventObject();
         obj.ObjectName = objType.Name;
         sod.SIF_EventObject = obj;
         evnt.SIF_ObjectData = sod;

         obj.Action = EventAction.Add.ToString();

         Object eventObject = null;
         try
         {
            eventObject = ClassFactory.CreateInstance(objType.FQClassName);
         }
         catch (Exception cfe)
         {
            throw new AdkException("Unable to create instance of " + objType.Name, fZone, cfe);
         }


         obj.AddChild(objType, (SifElement)eventObject);

         return evnt;
      }


      // TODO: Implement these test methods from the Java ADK unit tests
      /*
    public void testQueryResults() throws ADKException
                            {
                               TestState requestState = new TestState( ADK.makeGUID() );
    ElementDef objType = SifDtd.STUDENTCONTACT;
    ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_NORMAL_HANDLING );
    handler.RequestStateObject = requestState;
		
    fZone.setQueryResults( handler, objType );
		
    SIF_Response r = createSIF_Response( objType, true, requestState );
    assertNormalHandling( handler, r, fZone );
    assertRequestCacheCleared( r );
 }
	
 public void testQueryResultsTwoPackets() throws ADKException
{
 TestState requestState = new TestState( ADK.makeGUID() );
 ElementDef objType = SifDtd.STUDENTCONTACT;
 ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_NORMAL_HANDLING );
 handler.RequestStateObject = requestState;
		
 fZone.setQueryResults( handler, objType );
		
 SIF_Response r = createSIF_Response( objType, true, requestState );
 // Process first packet
 r.setSIF_PacketNumber( "1" );
 r.setSIF_MorePackets( "Yes" );
 assertNormalHandling( handler, r, fZone );
		
 // Process first packet
 r.setSIF_PacketNumber( "2" );
 r.setSIF_MorePackets( "No" );
 assertNormalHandling( handler, r, fZone );
		
 assertRequestCacheCleared( r );
		
}
	
	
 public void testQueryResultsTwoPacketsFirstError() throws ADKException
{
 TestState requestState = new TestState( ADK.makeGUID() );
 ElementDef objType = SifDtd.STUDENTCONTACT;
 ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_THROW_EXCEPTION );
 handler.RequestStateObject = requestState;
		
 fZone.setQueryResults( handler, objType );
		
 SIF_Response r = createSIF_Response( objType, true, requestState );
 // Process first packet
 r.setSIF_PacketNumber( "1" );
 r.setSIF_MorePackets( "Yes" );
		
 // The first packet is going to throw an exception
 boolean exceptionThrown = false;
 try
{
 assertNormalHandling( handler, r, fZone );
}
 catch( Exception ex )
{
 exceptionThrown = true;
}
		
 assertEquals( "An Exception should have been thrown", true, exceptionThrown );
		
 // Process second packet
 r.setSIF_PacketNumber( "2" );
 r.setSIF_MorePackets( "No" );
 handler.Behavior = ErrorMessageHandler.BVR_NORMAL_HANDLING;
 assertNormalHandling( handler, r, fZone );
		
 // Now the RequestCache should no longer contain the specified object
 assertRequestCacheCleared( r );
		
}
	
 public void testQueryResultsTwoPacketsFirstRetry() throws ADKException
{
 TestState requestState = new TestState( ADK.makeGUID() );
 ElementDef objType = SifDtd.STUDENTCONTACT;
 ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_THROW_ADKEXCEPTION_RETRY );
 handler.RequestStateObject = requestState;
		
 fZone.setQueryResults( handler, objType );
		
 SIF_Response r = createSIF_Response( objType, true, requestState );
 // Process first packet
 r.setSIF_PacketNumber( "1" );
 r.setSIF_MorePackets( "Yes" );
		
 // The first packet is going to throw an exception with a retry
 assertRetryHandling( handler, r, fZone );
		
 // Now process it again without an exception
 handler.Behavior = ErrorMessageHandler.BVR_NORMAL_HANDLING;
 assertNormalHandling(handler, r, fZone);
		
 // Process second packet
 r.setSIF_PacketNumber( "2" );
 r.setSIF_MorePackets( "No" );
 handler.Behavior = ErrorMessageHandler.BVR_NORMAL_HANDLING;
 assertNormalHandling( handler, r, fZone );
		
 // Now the RequestCache should no longer contain the specified object
 assertRequestCacheCleared( r );
		
}
 */


      [Test]
      public void testADKRetryQueryResults()
      {
         TestState requestState = new TestState(Adk.MakeGuid());
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowADKRetryException);
         handler.RequestStateObject = requestState;

         fZone.SetQueryResults(handler, objType, null );
         fZone.Connect(ProvisioningFlags.Register);
         SIF_Response r = createSIF_Response(objType, true, requestState);
         AssertRetryHandling(handler, r, fZone);

         // Now, dispatch a second time. This time the dispatching should work correctly, including
         // custom state
         handler.Behavior = ErrorMessageHandler.HandlerBehavior.Normal;
         assertNormalHandling(handler, r, fZone);
         assertRequestCacheCleared(r);
      }

      [Test]
      public void testQueryResultsNoCache()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler = new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.Normal);

         fZone.SetQueryResults(handler, objType, null );
         fZone.Connect(ProvisioningFlags.Register);
         SIF_Response r = createSIF_Response(objType, false, null);
         assertNormalHandling(handler, r, fZone);
         assertRequestCacheCleared(r);
      }

      [Test]
      public void testADKRetryQueryResultsNoCache()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowADKRetryException);

         fZone.SetQueryResults(handler, objType, null );
         fZone.Connect(ProvisioningFlags.Register);
         SIF_Response r = createSIF_Response(objType, false, null);

         AssertRetryHandling(handler, r, fZone);

         // Now, dispatch a second time. This time the dispatching should work correctly, including
         // custom state
         handler.Behavior = ErrorMessageHandler.HandlerBehavior.Normal;
         assertNormalHandling(handler, r, fZone);
         assertRequestCacheCleared(r);
      }

      [Test]
      public void testSIFRetryQueryResultsNoCache()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowSIFRetryException);

         fZone.SetQueryResults(handler, objType, null );
         fZone.Connect(ProvisioningFlags.Register);
         SIF_Response r = createSIF_Response(objType, false, null);
         AssertRetryHandling(handler, r, fZone);

         // Now, dispatch a second time. This time the dispatching should work correctly, including
         // custom state
         handler.Behavior = ErrorMessageHandler.HandlerBehavior.Normal;
         assertNormalHandling(handler, r, fZone);
         assertRequestCacheCleared(r);
      }


      [Test]
      public void testSIFRetryQueryResults()
      {
         TestState requestState = new TestState(Adk.MakeGuid());
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowSIFRetryException);
         handler.RequestStateObject = requestState;
         fZone.SetQueryResults(handler, objType, null);
         fZone.Connect(ProvisioningFlags.Register);
         SIF_Response r = createSIF_Response(objType, true, requestState);
         AssertRetryHandling(handler, r, fZone);

         // Now, dispatch a second time. This time the dispatching should work correctly, including
         // custom state
         handler.Behavior = ErrorMessageHandler.HandlerBehavior.Normal;
         assertNormalHandling(handler, r, fZone);
         assertRequestCacheCleared(r);
      }


      [Test]
      public void testADKRetryEvent()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler = new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowADKRetryException);

         // SifContext ctxt = SifContext.Create("SIF_SUBSCRIBE");
         // SubscriptionOptions options = new SubscriptionOptions(ctxt);

         fZone.SetSubscriber(handler, objType, null);
         SIF_Event evnt = createSIF_Event(objType);

         fZone.Connect(ProvisioningFlags.Register);
         AssertRetryHandling(handler, evnt, fZone);
      }


            [Test]
      public void testADKRetryPublish()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowADKRetryException);
         fZone.SetPublisher(handler, objType, null);
         fZone.Connect(ProvisioningFlags.Register);
         AssertRetryHandling(handler, createSIF_Request(objType), fZone);
      }

      [Test]
      public void testSIFRetryPublish()
      {
         IElementDef objType = StudentDTD.STUDENTCONTACT;
         ErrorMessageHandler handler =
             new ErrorMessageHandler(ErrorMessageHandler.HandlerBehavior.ThrowSIFRetryException);
         fZone.SetPublisher(handler, objType, null);
         fZone.Connect(ProvisioningFlags.Register);
         AssertRetryHandling(handler, createSIF_Request(objType), fZone);
      }

      // TODO: Implement
      /*
    [Test]
    public void testReportPublishSIFExceptionAfterReportInfo() 
    {
    ElementDef objType = SifDtd.SIF_REPORTOBJECT;
    ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_SET_REPORT_INFO_THROW_EXCEPTION );
    fZone.setReportPublisher( handler, ADKFlags.PROV_NONE);
		
    TestProtocolHandler testProto = new TestProtocolHandler();
    testProto.open(fZone);
    MessageDispatcher testDispatcher = new MessageDispatcher( fZone );
    fZone.setDispatcher(testDispatcher);
    fZone.setProto(testProto);
    testDispatcher.dispatch( createSIF_Request( objType, ADK.makeGUID(), fZone ) );
    String msg = testProto.readMsg();
    assertNull(testProto.readMsg());
    fZone.log.info(msg);

    SIFParser parser = SIFParser.newInstance();
    SIFElement element = parser.parse(new StringReader(msg), fZone);
    assertTrue(element instanceof SIF_Response);
    SIF_Response response = (SIF_Response) element;
    assertTrue(response.getSIF_Error() != null);
    assertTrue(response.getSIF_Error().getSIF_Desc().startsWith("Blah"));
 }
	
    public void testReportPublishSIFExceptionAfterReportInfo() throws ADKException, IOException
 {
    ElementDef objType = SifDtd.SIF_REPORTOBJECT;
    ErrorMessageHandler handler = new ErrorMessageHandler( ErrorMessageHandler.BVR_SET_REPORT_INFO_THROW_EXCEPTION );
    fZone.setReportPublisher( handler, ADKFlags.PROV_NONE);
		
    TestProtocolHandler testProto = new TestProtocolHandler();
    testProto.open(fZone);
    MessageDispatcher testDispatcher = new MessageDispatcher( fZone );
    fZone.setDispatcher(testDispatcher);
    fZone.setProto(testProto);
    testDispatcher.dispatch( createSIF_Request( objType, ADK.makeGUID(), fZone ) );
    String msg = testProto.readMsg();
    assertNull(testProto.readMsg());
    fZone.log.info(msg);

    SIFParser parser = SIFParser.newInstance();
    SIFElement element = parser.parse(new StringReader(msg), fZone);
    assertTrue(element instanceof SIF_Response);
    SIF_Response response = (SIF_Response) element;
    assertTrue(response.getSIF_Error() != null);
    assertTrue(response.getSIF_Error().getSIF_Desc().startsWith("Blah"));
 }
		
*/

      private SIF_Request createSIF_Request(IElementDef objType)
      {
         SIF_Request request = new SIF_Request();
         request.Header.SIF_MsgId = MSG_GUID;
         request.Header.SIF_SourceId = "foo";
         request.SIF_MaxBufferSize = 32768;
         request.AddSIF_Version(new SIF_Version(Adk.SifVersion.ToString()));
         SIF_Query q = new SIF_Query();
         SIF_QueryObject sqo = new SIF_QueryObject();
         sqo.ObjectName = objType.Name;

         q.SIF_QueryObject = sqo;
         request.SIF_Query = q;

         return request;
      }

      // TODO: Implement
      /*

    private SIF_Request createSIF_Request( ElementDef objectType, String refId, Zone zone )
    {
       SIF_Request request = new SIF_Request();
       request.getHeader().setSIF_MsgId( MSG_GUID );
       request.getHeader().setSIF_SourceId( "foo" );
       request.setSIF_MaxBufferSize("32768");
       request.setSIF_Version( ADK.getSIFVersion().toString() );
       Query query = new Query(objectType);
       query.addCondition(SifDtd.SIF_REPORTOBJECT_REFID, Condition.EQ, refId);

       SIF_Query q = SIFPrimitives.createSIF_Query(query, zone);
       SIF_QueryObject sqo = new SIF_QueryObject();
       sqo.setObjectName( objectType.name() );
		
       q.setSIF_QueryObject(sqo);
       request.setSIF_Query(q);
		
       return request;
    }
	
 */


      private SIF_Response createSIF_Response(IElementDef objType, bool storeInRequestCache, ISerializable stateObject)
      {
         SIF_Request req = createSIF_Request(objType);

         if (storeInRequestCache)
         {
            Query q = new Query(objType);
            q.UserData = stateObject;
            RequestCache.GetInstance(fAgent).StoreRequestInfo(req, q, fZone);
         }

         SIF_Response resp = new SIF_Response();
         resp.SIF_RequestMsgId = req.Header.SIF_MsgId;
         SIF_ObjectData sod = new SIF_ObjectData();
         resp.SIF_ObjectData = sod;

         Object responseObject = null;
         try
         {
            responseObject = ClassFactory.CreateInstance(objType.FQClassName, false);
         }
         catch (Exception cfe)
         {
            throw new AdkException("Unable to create instance of " + objType.Name, fZone, cfe);
         }

         sod.AddChild((SifElement)responseObject);
         return resp;
      }


      private void assertNormalHandling(ErrorMessageHandler handler, SifMessagePayload payload, ZoneImpl zone)
      {
         MessageDispatcher testDispatcher = new MessageDispatcher(zone);
         int result = testDispatcher.dispatch(payload);
         Assert.IsTrue(handler.wasCalled(), "Handler was not called");
         Assert.AreEqual(1, result, "Result code should always be 1 because this version does not support SMB");
      }

      private void AssertExceptionHandling(ErrorMessageHandler handler, SifMessagePayload payload, ZoneImpl zone,
                                             Type expectedExceptionType)
      {
         Exception exc = null;
         try
         {
            assertNormalHandling(handler, payload, zone);
         }
         catch (Exception ex)
         {
            exc = ex;
            Assert.IsTrue(handler.wasCalled(), "Handler was not called");

            AdkMessagingException adkme = ex as AdkMessagingException;
            Assert.IsNotNull(adkme,
                             "Expected an ADKMessagingException, but was " + ex.GetType().Name + ":" + ex.ToString());

             Exception source = adkme;
             Exception innerEx = null;
             while( source.InnerException != null )
             {
                 innerEx = source.InnerException;
                 source = innerEx;
             }
            Assert.IsNotNull(innerEx, "AdkMessaginException was thrown but inner exception was not set");

            if (innerEx.GetType() != expectedExceptionType)
            {
               Assert.Fail("Exception thrown was not a " + expectedExceptionType.Name + ", but was " +
                           innerEx.GetType().Name + ":" + innerEx.ToString());
            }
         }

         Assert.IsNotNull(exc, "An exception was not thrown by the handler");
         AssertThreadIsOK();
      }

      private void AssertThreadIsOK()
      {
         try
         {
            Thread.Sleep(10);
         }
         catch (Exception ex)
         {
            Assert.Fail("Tried to sleep the thread, but an Exception was thrown: " + ex.GetType().Name + " : " +
                        ex.ToString());
         }
      }


      private void AssertRetryHandling(ErrorMessageHandler handler, SifMessagePayload payload, ZoneImpl zone)
      {
         try
         {
            assertNormalHandling(handler, payload, zone);
         }
         catch (SifException ex)
         {
            Assert.IsTrue(handler.wasCalled(), "Handler was not called");
            Assert.AreEqual(SifErrorCategoryCode.Transport, ex.ErrorCategory,
                            "SIF Error category should be 10: " + ex.Message);
         }
         Assert.IsTrue(handler.wasCalled(), "Handler was not called");
         AssertThreadIsOK();
      }

      private void assertRequestCacheCleared(SIF_Response r)
      {
         // Now the RequestCache should no longer contain the specified object
         IRequestInfo inf = RequestCache.GetInstance(fAgent).LookupRequestInfo(r.SIF_RequestMsgId, fZone);
         Assert.IsNull(inf, "RequestInfo should be removed from the cache");
      }
   }
}