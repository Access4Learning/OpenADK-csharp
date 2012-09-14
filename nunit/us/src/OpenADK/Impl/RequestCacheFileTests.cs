using System;
using System.IO;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Impl
{
    [TestFixture]
    public class RequestCacheFileTests
    {
        private String[] fStateObjects;
        private String[] fMsgIds;
        private RequestCache fRC;
        private Agent fAgent;

        [SetUp]
        public void setUp()
        {
            Adk.Initialize();

            fAgent = new TestAgent();
            fAgent.Initialize();
        }

        [TearDown]
        public void tearDown()
        {
            if (fRC != null)
            {
                fRC.Close();
                fRC = null;
            }

            String fname = fAgent.HomeDir + Path.DirectorySeparatorChar + "work" + Path.DirectorySeparatorChar +
                           "requestcache.adk";
            File.Delete(fname);
            //File f = new File(fname);
            //f.delete();
        }


        /**
       * Tests that the RequestCache file persists information between requests
       * @throws Exception
       */

        [Test]
        public void testSimpleCase()
        {
            fRC = RequestCache.GetInstance(fAgent);
            storeAssertedRequests(fRC);
            assertStoredRequests(fRC, true);
        }

        /**
       * Tests that the RequestCache file persists information between restarts
       * @throws Exception
       */

        [Test]
        public void testPersistence()
        {
            fRC = RequestCache.GetInstance(fAgent);
            storeAssertedRequests(fRC);
            fRC.Close();

            // Create a new instance. This one should retrieve its settings from the persistence mechanism
            fRC = RequestCache.GetInstance(fAgent);
            assertStoredRequests(fRC, true);

            fRC.Close();
            fRC = RequestCache.GetInstance(fAgent);
            Assertion.AssertEquals("Should have zero pending requests", 0, fRC.ActiveRequestCount);
        }


        /**
       * Tests that the RequestCache file persists information between restarts
       * Even if the state object is not able to be deserialized
       * @throws Exception
       */

        [Test]
        public void testPersistenceWithBadState()
        {
            //create new cache for agent
            RequestCache cache = RequestCache.GetInstance(fAgent);

            //create new queryobject
            SIF_QueryObject obj = new SIF_QueryObject("");
            //create query, telling it what type of query it is(passing it queryobj)
            SIF_Query query = new SIF_Query(obj);
            //create new sif request
            SIF_Request request = new SIF_Request();
            //set query property
            request.SIF_Query = query;


            Query q = new Query(StudentDTD.STUDENTPERSONAL);

            String testStateItem = Adk.MakeGuid();
            String requestMsgId = Adk.MakeGuid();
            String testObjectType = Adk.MakeGuid();

            TestState ts = new TestState();
            ts.State = testStateItem;
            ts.setCreateErrorOnRead(true);

            q.UserData = ts;
            storeRequest(cache, request, q, requestMsgId, testObjectType);

            cache.Close();

            // Create a new instance. This one should retrieve its settings from the persistence mechanism
            cache = RequestCache.GetInstance(fAgent);

            IRequestInfo ri = cache.GetRequestInfo(requestMsgId, null);

            //if state is null, should still return ri object
            Assertion.AssertNotNull("RequestInfo was null", ri);
            Assertion.AssertEquals("MessageId", requestMsgId, ri.MessageId);
            Assertion.AssertEquals("ObjectType", testObjectType, ri.ObjectType);
            ts = (TestState) ri.UserData;
            // In order for this to be a valid test, the TestState class should have thrown
            // an exception during deserialization and should be null here.
            Assertion.AssertNull("UserData should be null", ts);
        }

        [Test]
        public void testInstanceMultipleInvocations()
        {
            for (int i = 0; i < 3; i++)
            {
                testPersistence();
            }
        }

        [Test]
        public void testPersistenceMultipleInvocations()
        {
            for (int i = 0; i < 3; i++)
            {
                testSimpleCase();
            }
        }

        [Test]
        public void testPersistenceWithRemoval()
        {
            fRC = RequestCache.GetInstance(fAgent);
            SIF_QueryObject obj = new SIF_QueryObject("");
            SIF_Query query = new SIF_Query(obj);
            SIF_Request request = new SIF_Request();

            request.SIF_Query = query;


            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            String testStateItem = Adk.MakeGuid();
            TestState ts = new TestState();
            ts.State = testStateItem;
            q.UserData = ts;

            fMsgIds = new String[10];
            // Add 10 entries to the cache, interspersed with other entries that are removed
            for (int i = 0; i < 10; i++)
            {
                String phantom1 = Adk.MakeGuid();
                String phantom2 = Adk.MakeGuid();
                storeRequest(fRC, request, q, phantom1, "foo");
                fMsgIds[i] = Adk.MakeGuid();
                storeRequest(fRC, request, q, fMsgIds[i], "Object_" + i);
                storeRequest(fRC, request, q, phantom2, "bar");

                fRC.GetRequestInfo(phantom1, null);
                fRC.GetRequestInfo(phantom2, null);
            }

            // remove every other entry, close, re-open and assert that the correct entries are there
            for (int i = 0; i < 10; i += 2)
            {
                fRC.GetRequestInfo(fMsgIds[i], null);
            }

            Assertion.AssertEquals("Before closing Should have five objects", 5, fRC.ActiveRequestCount);
            fRC.Close();

            // Create a new instance. This one should retrieve its settings from the persistence mechanism
            fRC = RequestCache.GetInstance(fAgent);
            Assertion.AssertEquals("After Re-Openeing Should have five objects", 5, fRC.ActiveRequestCount);
            for (int i = 1; i < 10; i += 2)
            {
                IRequestInfo cachedInfo = fRC.GetRequestInfo(fMsgIds[i], null);
                Assertion.AssertNotNull("No cachedID returned for " + i, cachedInfo);
            }
            Assertion.AssertEquals("Should have zero objects", 0, fRC.ActiveRequestCount);
        }


        /**
       * Tests that the RequestCacheFile class handles the case of the 
       * cache file becoming readonly
       * @throws Exception
       */

        [Test]
        public void testWithReadOnlyFile()
        {
            // Make the existing cache file readonly
            String fname = fAgent.HomeDir + Path.DirectorySeparatorChar + "work" + Path.DirectorySeparatorChar +
                           "requests.adk";
            FileInfo fi = new FileInfo(fname);

            //= new File(fname);
            if (!fi.Exists)
            {
                StreamWriter sw = fi.CreateText();
                //sw.WriteLine("");
                sw.Flush();
                sw.Close();

                // RandomAccessFile raf = new RandomAccessFile(fname, "rw");
                // raf.setLength(0);
                //raf.close();
            }
            fi.IsReadOnly = true;
            try
            {
                fRC = RequestCache.GetInstance(fAgent); //this should throw adk exception
            }
            catch (AdkException)
            {
                return;
            }
            finally
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
            //should never get here

            Assertion.AssertEquals("Exception should have been thrown because request cache file is readonly", true,
                                   false);
        }


        /**
       * Tests that the RequestCache file persists information between restarts, even if it starts
       * with a corrupt file
       * @throws Exception
       */

        [Test]
        public void testWithCorruptFile()
        {
            // Delete the existing cache file, if it exists
            String fname = fAgent.HomeDir + Path.DirectorySeparatorChar + "work" + Path.DirectorySeparatorChar +
                           "requestcache.adk";
            FileInfo fi = new FileInfo(fname);
            StreamWriter sw = fi.CreateText();
            sw.WriteLine("!@#$!@#$");
            sw.Flush();
            sw.Close();

            //RandomAccessFile raf = new RandomAccessFile(fname, "rw");
            //raf.writeChars("!@#$!@#$");
            // raf.close();

            fRC = RequestCache.GetInstance(fAgent);
            storeAssertedRequests(fRC);
            fRC.Close();

            // Create a new instance. This one should retrieve its settings from the persistence mechanism
            fRC = RequestCache.GetInstance(fAgent);
            assertStoredRequests(fRC, true);
        }

        [Test]
        public void testWithLegacyFile()
        {
            //assertStoredRequests(fRC, true);
            // Copy the legacy requests.adk file to the agent work directory
            //FileInfo legacyFile = new FileInfo("requests.adk");

            //Assertion.Assert("Saved legacy file does [not?] exist", legacyFile.Exists);
            //FileInfo copiedFile = new FileInfo(fAgent.HomeDir + Path.DirectorySeparatorChar + "work" + Path.DirectorySeparatorChar + "requests.adk");
            //if (copiedFile.Exists)
            //{
            //   copiedFile.Delete();
            //}

            //// Copy the file
            //legacyFile.CopyTo(copiedFile.FullName, true);

            // Now open up an instance of the request cache and verify that the contents are there


            fRC = RequestCache.GetInstance(fAgent);
            SIF_QueryObject obj = new SIF_QueryObject("");
            SIF_Query query = new SIF_Query(obj);
            SIF_Request request = new SIF_Request();
            request.SIF_Query = query;

            Query q;
            TestState ts;

            fMsgIds = new String[10];
            fStateObjects = new String[10];
            // Add 10 entries to the cache 
            for (int i = 0; i < 10; i++)
            {
                ts = new TestState();
                ts.State = Adk.MakeGuid();
                fStateObjects[i] = (String) ts.State;
                q = new Query(StudentDTD.STUDENTPERSONAL);
                q.UserData = ts;
                fMsgIds[i] = Adk.MakeGuid();
                storeRequest(fRC, request, q, fMsgIds[i], "Object_" + i.ToString());
            }


            Assertion.AssertEquals("Active request count", 10, fRC.ActiveRequestCount);


            // Lookup each setting, 
            for (int i = 0; i < 10; i++)
            {
                IRequestInfo reqInfo = fRC.LookupRequestInfo(fMsgIds[i], null);
                Assertion.AssertEquals("Initial lookup", "Object_" + i.ToString(), reqInfo.ObjectType);
            }

            // Lookup each setting, 
            for (int i = 0; i < 10; i++)
            {
                IRequestInfo reqInfo = fRC.GetRequestInfo(fMsgIds[i], null);
                Assertion.AssertEquals("Initial lookup", "Object_" + i.ToString(), reqInfo.ObjectType);
            }

            // all messages should now be removed from the queue
            Assertion.AssertEquals("Cache should be empty", 0, fRC.ActiveRequestCount);

            // Now run one of our other tests
            testPersistence();
        }


        /**
       * Stores the items in the cache that will later be asserted
       * @param cache
       */

        private void storeAssertedRequests(RequestCache cache)
        {
            SIF_QueryObject obj = new SIF_QueryObject("");
            SIF_Query query = new SIF_Query(obj);
            SIF_Request request = new SIF_Request();
            request.SIF_Query = query;

            Query q;
            TestState ts;

            fMsgIds = new String[10];
            fStateObjects = new String[10];
            // Add 10 entries to the cache, interspersed with other entries that are removed
            for (int i = 0; i < 10; i++)
            {
                ts = new TestState();
                ts.State = Adk.MakeGuid();
                fStateObjects[i] = ts.State;
                q = new Query(StudentDTD.STUDENTPERSONAL);
                q.UserData = ts;

                String phantom1 = Adk.MakeGuid();
                String phantom2 = Adk.MakeGuid();
                storeRequest(cache, request, q, phantom1, "foo");
                fMsgIds[i] = Adk.MakeGuid();

                storeRequest(cache, request, q, fMsgIds[i], "Object_" + i.ToString());
                storeRequest(cache, request, q, phantom2, "bar");

                cache.GetRequestInfo(phantom1, null);
                cache.GetRequestInfo(phantom2, null);
            }
        }


        private void storeRequest(
            RequestCache rc,
            SIF_Request request,
            Query q,
            String msgID,
            String objectName)
        {
            //request.getSIF_Query().getSIF_QueryObject().setObjectName(objectName);
            request.SIF_Query.SIF_QueryObject.ObjectName = objectName;
            request.Header.SIF_MsgId = msgID;
            rc.StoreRequestInfo(request, q, null);
        }


        /**
       * Asserts that the items stored in the storeAssertedRequests call
       * are still in the cache
       * @param cache The RequestCache class to assert
       * @param testRemoval True if the items in the cache should be removed
       * and asserted that they are removed
       */

        private void assertStoredRequests(RequestCache cache, Boolean testRemoval)
        {
            Assertion.AssertEquals("Active request count", fMsgIds.Length, cache.ActiveRequestCount);

            // Lookup each setting, 
            for (int i = 0; i < fMsgIds.Length; i++)
            {
                IRequestInfo reqInfo = cache.LookupRequestInfo(fMsgIds[i], null);
                Assertion.AssertEquals("Initial lookup", "Object_" + i.ToString(), reqInfo.ObjectType);
                Assertion.AssertEquals("User Data is missing for " + i, fStateObjects[i],
                                       (String) ((TestState) reqInfo.UserData).State);
            }

            if (testRemoval)
            {
                // Lookup each setting, 
                for (int i = 0; i < fMsgIds.Length; i++)
                {
                    IRequestInfo reqInfo = cache.GetRequestInfo(fMsgIds[i], null);
                    Assertion.AssertEquals("Initial lookup", "Object_" + i.ToString(), reqInfo.ObjectType);
                    Assertion.AssertEquals("User Data is missing for " + i, fStateObjects[i],
                                           (String) ((TestState) reqInfo.UserData).State);
                }

                // all messages should now be removed from the queue
                Assertion.AssertEquals("Cache should be empty", 0, cache.ActiveRequestCount);
            }
        }
    } //end class
} //end namespace