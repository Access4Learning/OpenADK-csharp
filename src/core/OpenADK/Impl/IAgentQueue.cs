//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Impl
{
    /// <summary>  The IAgentQueue interface is implemented by the Agent Local Queue. There
    /// are currently two implementations: one for file-based queuing and one for
    /// JDBC-based queuing.
    /// 
    /// 
    /// </summary>
    public interface IAgentQueue
    {
        /// <summary>  Is the queue ready? A queue is ready when it has been successfully
        /// initialized and shutdown has not been called.
        /// </summary>
        bool Ready { get; }

        /// <summary>  Gets the Zone that owns this queue</summary>
        IZone Zone { get; }

        /// <summary>  Initialize the queue</summary>
        /// <param name="agent">The agent that owns the queue. Each agent has one queue
        /// per zone to which it is connected.
        /// </param>
        /// <param name="zone">The zone that owns the queue. Each zone to which an agent
        /// is connected is represented by its own queue.
        /// </param>
        /// <param name="props">Implementation-specific initialization properties (e.g.
        /// location of the queue, user authentication parameters, etc.)
        /// </param>
        void Initialize( IZone zone,
                         IPropertyCollection props );

        /// <summary>  Close the queue</summary>
        void Shutdown();

        /// <summary>  Posts an unparsed incoming SIF message to the queue for later processing.</summary>
        void PostMessage( SifMessageInfo msgInfo );

        /// <summary>  Posts an outgoing SIF message to the queue</summary>
        void PostMessage( SifMessagePayload msg );

        /// <summary>  Removes a message from the queue</summary>
        void RemoveMessage( string msgId );

        /// <summary>  Gets the next available group of messages
        /// 
        /// </summary>
        /// <param name="msgType">The message type, or <c>MSG_ANY</c> to return the
        /// next available groups of messages regardless of type. Message type
        /// codes are defined by <c>MSG_</c> constants defined by the
        /// IAgentQueue interface.
        /// </param>
        /// <param name="direction">Specifies whether the message is incoming or outgoing;
        /// one of the following: <c>IAgentQueue.INCOMING</c>, <c>
        /// IAgentQueue.OUTGOING</c>, or <c>IAgentQueue.ALL</c>
        /// </param>
        SifMessageInfo [] nextMessage( SifMessageType msgType,
                                       MessageDirection direction );

        /// <summary>  Determines if a message is in the queue</summary>
        /// <param name="msgId">The SIF_MsgId identifier
        /// </param>
        bool HasMessage( string msgId );

        /// <summary>  Gets a message by ID
        /// 
        /// </summary>
        /// <param name="msgId">The message identifier
        /// </param>
        string GetMessage( string msgId );

        /// <summary>  Counts the total number of messages in the queue
        /// 
        /// </summary>
        /// <param name="msgType">The message type, or <c>MSG_ANY</c> to return the
        /// number of all messages regardless of type. Message type codes are
        /// defined by <c>MSG_</c> constants defined by the IAgentQueue
        /// interface.
        /// </param>
        /// <param name="direction">Specifies whether the message is incoming or outgoing;
        /// one of the following: <c>IAgentQueue.INCOMING</c>, <c>
        /// IAgentQueue.OUTGOING</c>, or <c>IAgentQueue.ALL</c>
        /// </param>
        int GetCount( SifMessageType msgType,
                      MessageDirection direction );

        //TODO: Implement Statistic when needed
        //			/// <summary>  Gets a queue statistic
        //			/// 
        //			/// </summary>
        //			/// <param name="id">Identifies the statistic to return. Identifiers are enumerated
        //			/// by STAT_ constants defined by the IAgentQueue interface.
        //			/// </param>
        //			/// <returns> A statistic object
        //			/// </returns>
        //			IStatistic getStatistic(sbyte statId);
    }
}
