using System;
using System.Threading;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Reporting;
using NUnit.Framework;

namespace Library.Nunit.US.Impl
{
    /// <summary>
    /// Summary description for ErrorMessageHandler.
    /// </summary>
    public class ErrorMessageHandler : ISubscriber, IQueryResults, IPublisher,
                                       IUndeliverableMessageHandler
    {
        [Flags]
        public enum HandlerBehavior
        {
            ThrowADKRetryException = 0x01,
            ThrowSIFRetryException = 0x02,
            Normal = 0x04,
            ThrowException = 0x08,
            SetReportInfoThrowException = 0x10,
            ThrowNullPointerException = 0x20,
            WaitForPulse = 0x40
        }

        private IRequestInfo fPendingRequestInfo;
        private IRequestInfo fResultsRequestInfo;
        private object fRequestStateObject;
        private HandlerBehavior fBehavior;
        private bool fWasCalled = false;
        private ManualResetEvent fWaitForObject;
        private ManualResetEvent fSignalObject;


        public ErrorMessageHandler(HandlerBehavior behavior)
        {
            fBehavior = behavior;
        }

        public ErrorMessageHandler(HandlerBehavior behavior, ManualResetEvent signalObject, ManualResetEvent waitObject)
            : this(behavior)
        {
            fWaitForObject = waitObject;
            fSignalObject = signalObject;
        }


        public void OnEvent(Event evnt, IZone zone, IMessageInfo info)
        {
            doBehavior(zone);
        }

        public void OnQueryPending(IMessageInfo info, IZone zone)
        {
            SifMessageInfo smi = (SifMessageInfo) info;
            fPendingRequestInfo = smi.SIFRequestInfo;
            // TODO: should we test error handling in the onQueryPending handler?
            //doBehavior( zone );

            Assert.IsNotNull(fPendingRequestInfo, "RequestInfo should not be null in onQueryPending()");
            if (RequestStateObject != null)
            {
                Assert.AreEqual(RequestStateObject, fPendingRequestInfo.UserData, "Custom State in onQueryPending()");
            }
        }

        public void OnQueryResults(IDataObjectInputStream data, SIF_Error error, IZone zone, IMessageInfo info)
        {
            SifMessageInfo smi = (SifMessageInfo) info;
            fResultsRequestInfo = smi.SIFRequestInfo;
            doBehavior(zone);

            Assert.IsNotNull(fResultsRequestInfo, "RequestInfo should not be null in onQueryResults()");
            if (RequestStateObject != null)
            {
                Assert.AreEqual(RequestStateObject, fResultsRequestInfo.UserData, "Custom State in onQueryResults()");
            }
        }


       public void OnRequest(IDataObjectOutputStream stream, Query query, IZone zone, IMessageInfo info)
        {
            doBehavior(zone);
        }

       public void OnReportRequest(String reportObjectRefId, IDataObjectOutputStream stream, Query query, IZone zone,
                                    IMessageInfo info)
        {
           doBehavior(zone);
            doBehavior(zone, reportObjectRefId, stream);
        }


        public bool wasCalled()
        {
            return fWasCalled;
        }

        private void doBehavior(IZone zone)
        {
            doBehavior(zone, null, null);
        }

        private void doBehavior(IZone zone, String reportObjectRefId, IDataObjectOutputStream reportStream)
        {
            fWasCalled = true;
            if ((Behavior & HandlerBehavior.WaitForPulse) != 0)
            {
                Console.WriteLine("Signaling...");
                fSignalObject.Set();
                Console.WriteLine("Waiting...");
                fWaitForObject.WaitOne();
                Console.WriteLine("Resuming publishing...");
            }

            if ((Behavior & HandlerBehavior.ThrowException) != 0)
            {
                AdkException exc = new AdkException("Errors Occurred", zone);
                throw exc;
            }
            if ((Behavior & HandlerBehavior.ThrowADKRetryException) != 0)
            {
                AdkException exc = new AdkException("Errors Occurred", zone);
                exc.Retry = true;
                throw exc;
            }
            if ((Behavior & HandlerBehavior.ThrowSIFRetryException) != 0)
            {
                AdkException exc =
                    new SifException(SifErrorCategoryCode.Transport, SifErrorCodes.GENERIC_GENERIC_ERROR_1,
                                     "Errors Occurred", zone);
                exc.Retry = true;
                throw exc;
            }
            if ((Behavior & HandlerBehavior.ThrowNullPointerException) != 0)
            {
                throw new NullReferenceException("Bogus!");
            }
        }

        public Object RequestStateObject
        {
            get { return fRequestStateObject; }
            set { fRequestStateObject = value; }
        }

        public HandlerBehavior Behavior
        {
            get { return fBehavior; }
            set { fBehavior = value; }
        }


        public bool OnDispatchError(SifMessagePayload message, IZone zone, IMessageInfo info)
        {
            try
            {
                doBehavior(zone);
            }
            catch (SifException sifex)
            {
                throw sifex;
            }
            catch (AdkException adke)
            {
                throw new SifException(0, 0, "This shouldn't happen", zone, adke);
            }
            return false;
        }
    }
}