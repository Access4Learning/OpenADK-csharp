//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>  The QueryResults interface is implemented by classes that process the
    /// results of queries initiated by the agent. Agents typically register a
    /// <i>QueryResults</i> implementation with a Topic or Zone.
    /// 
    /// 
    /// <b>Pending Queries</b>
    /// 
    /// The <code>onQueryPending</code> method is called by the ADK for each zone to
    /// which a SIF_Request message is sent. Agents interested in tracking the
    /// results of pending queries may implement this method to match SIF_Request
    /// messages with SIF_Response messages. Agents that do not require this
    /// functionality can provide an empty implementation.
    /// 
    /// 
    /// <b>Query Results</b>
    /// 
    /// The <code>onQueryResults</code> method is called whenever a SIF_Response
    /// message is received from a zone. The ADK handles extracting the individual
    /// SIF Data Objects from the message and streaming them to the QueryResultsInputStream
    /// passed as a parameter to this method. Agents should expect to receive
    /// arbitrarily large result sets from the stream.
    /// 
    /// 
    /// <b>Message Dispatching Chain</b>
    /// 
    /// Because SIF is an asynchronous messaging architecture, result data may be
    /// received at any time, including results for queries executed in prior agent
    /// sessions. When a SIF_Response message is received, the ADK dispatches the
    /// result set to the first <i>QueryResults</i> implementation in the message
    /// dispatching chain. The chain consists of Topic, Zone, and Agent in that order.
    /// If no <i>QueryResults</i> object is registered with any of these objects, the
    /// SIF_Response message is silently discarded. To avoid this situation, consider
    /// registering a "catch-all" <i>QueryResults</i> implementation with your Agent.
    /// You may also want to check the timestamp of messages by inspecting the
    /// MessageInfo parameter passed to the <code>onQueryResults</code> method.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  ADK 1.0
    /// </version>
    public interface IQueryResults
    {
        /// <summary>  Called by the ADK after successfully sending a SIF_Request message to a
        /// zone. If it is important to your implementation class to match pending
        /// SIF_Request queries SIF_Responses, you can use this method to keep track
        /// of the pending SIF_Requests.
        /// 
        /// Note this method is called once for each Zone a SIF_Request message is
        /// sent to, and is called only after the SIF_Request message is successfully
        /// sent.
        /// 
        /// </summary>
        /// <param name="info">The properties of the SIF_Request message. Cast this
        /// value to a SifMessageInfo object to retrieve attributes specific to
        /// the Schools Interoperability Framework protocol.
        /// </param>
        /// <param name="zone">The zone to which the SIF_Request message was sent.
        /// </param>
        void OnQueryPending( IMessageInfo info,
                             IZone zone );

        /// <summary>  Called by the ADK when it receives a SIF_Response message containing the
        /// results of a SIF_Request query previously issued by the agent. Note that
        /// query responses are received asynchronously and may not be received for
        /// a long time after the SIF_Request was issued, if at all.
        /// 
        /// 
        /// An implementation of this method should check the <i>error</i> parameter
        /// to determine if the SIF_Response message contains an error. The responder
        /// to the query may return an error if it doesn't support the object type
        /// associated with the query, or if it failed to execute the query. The
        /// SIF_Error object provides the error category, error code, description,
        /// and optionally an extended description. The <i>data</i> stream should not
        /// be expected to return any objects.
        /// 
        /// </summary>
        /// <param name="data">A DataObjectInputStream that provides all SIFDataObjects
        /// contained in the SIF_Response message
        /// </param>
        /// <param name="error">If the SIF_Response message contains an error, this
        /// parameter is the SIF_Error element from that message; otherwise it
        /// is null
        /// </param>
        /// <param name="zone">The zone from which the SIF_Response message was received.
        /// </param>
        /// <param name="info">The properties of the SIF_Request message. Cast this
        /// value to a SifMessageInfo object to retrieve attributes specific to
        /// the Schools Interoperability Framework protocol.
        /// </param>
        void OnQueryResults( IDataObjectInputStream data,
                             SIF_Error error,
                             IZone zone,
                             IMessageInfo info );
    }
}
