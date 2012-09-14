//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
   /// <summary>
   /// Represents informaton about an outstanding SIF_Request that has been made to the zone
   /// </summary>
   [Serializable]
   public class RequestCacheFileEntry : ISerializable, IRequestInfo
   {
      private string fObjType;
      private string fMessageId;
      private DateTime fRequestTime;
      [NonSerialized]
      private object fState;

      /// <summary>
      /// The offset that this entry is stored in the file. This value is not serialized
      /// because it is written to the stream seperately
      /// </summary>
      private long fOffset;

      /// <summary>
      /// Whether or not this item represents an active request. This value is not serialized
      /// because it is written to the stream seperately
      /// </summary>
      private bool fIsActive = true;

      /// <summary>
      /// Creates an Entry instance
      /// </summary>
      /// <param name="active"></param>
      internal RequestCacheFileEntry(bool active)
      {
         fIsActive = active;
      }

      /// <summary>
      /// returns current value of fState
      /// This property gets serialized separately from this class
      /// so that if read fails upon deserialization due to State,
      /// the rest of the class still gets returned.
      /// </summary>
      /// <param name="active"></param>

      public object State
      {
         get
         {
            return fState;
         }
         set
         {
            fState = value;
         }
      }


      /// <summary>
      /// The .Net Serialization constructor
      /// </summary>
      /// <param name="info"></param>
      /// <param name="context"></param>
      [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
      private RequestCacheFileEntry(SerializationInfo info,
                                     StreamingContext context)
      {
         fObjType = info.GetString("fObjType");
         fMessageId = info.GetString("fMessageId");
         fRequestTime = info.GetDateTime("fRequestTime");

         // fState is set by second desserialize call,
         //which sets State Property
         //try
         //{
         //   fState = info.GetValue("fState", typeof(Object));
         //}
         //catch (Exception ex)
         //{
         //   Agent.Log.Error
         //       ("Error retrieving custom UserData from RequestCache: " + ex.Message, ex);
         //}
      }

      void ISerializable.GetObjectData(SerializationInfo info,
                                        StreamingContext context)
      {
         info.AddValue("fObjType", fObjType);
         info.AddValue("fMessageId", fMessageId);
         info.AddValue("fRequestTime", fRequestTime);
         //try
         //{
         //   info.AddValue("fState", fState);
         //}
         //catch (Exception ex)
         //{
         //   Agent.Log.Error
         //       ("Error storing custom UserData to RequestCache: " + ex.Message, ex);
         //}
      }

      /// <summary>
      /// The Object Type of the Request. e.g. "StudentPersonal"
      /// </summary>
      public string ObjectType
      {
         get { return fObjType; }
      }

      internal void SetObjectType(string objType)
      {
         fObjType = objType;
      }

      /// <summary>
      /// The SIF_Request MessageId
      /// </summary>
      public string MessageId
      {
         get { return fMessageId; }
      }

      internal void SetMessageId(string messageId)
      {
         fMessageId = messageId;
      }

      /// <summary>
      /// The Date and Time that that this request was initially made
      /// </summary>
      public DateTime RequestTime
      {
         get { return fRequestTime; }
      }

      internal void SetRequestTime(DateTime requestTime)
      {
         fRequestTime = requestTime;
      }


      /// <summary>
      /// Returns whether or not this Request is Active
      /// </summary>
      public bool IsActive
      {
         get { return fIsActive; }
      }

      internal void SetIsActive(bool active)
      {
         fIsActive = active;
      }

      /// <summary>
      /// Returns the Serializable UserData state object that was placed in the 
      /// <see cref="OpenADK.Library.Queries"/> query class at the time of the original request.
      /// </summary>
      public object UserData
      {
         get { return fState; }
      }

      internal void SetUserData(object userData)
      {
         fState = userData;
      }

      /// <summary>
      /// Used internally by the ADK to track the persistant location of the 
      /// RequestInfo
      /// </summary>
      internal long Location
      {
         get { return fOffset; }
         set { fOffset = value; }
      }
   }
}
