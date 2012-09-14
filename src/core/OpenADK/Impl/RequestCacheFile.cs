//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  A RequestCache implementation that stores SIF_Request information to a file
    /// in the agent work directory.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    internal class RequestCacheFile : RequestCache
    {
        private Hashtable fCache = new Hashtable();
        private FileStream fFile;
        private BinaryFormatter fFormatter;
        /// <summary>
        /// The Entry class is serialized to the Request cache file
        /// </summary>
        /// <remarks>
        /// The format of the RandomAccess file is arranged as follows:
        /// <code>
        /// 0x00 A single byte with the values:
        ///         '1' indicating an active record
        ///         '0' indicating a deleted record
        /// 0x01 8 bytes indicating the length of the serialized Entry as
        ///      an unsigned long
        /// 0x09 The start of the serialized entry object.  
        /// </code>
        /// </remarks>
        protected internal override void Initialize(Agent agent)
        {
            Initialize(agent, false);
        }

        /// <summary>  Initialize the RequestCache</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Initialize(Agent agent,
                                 bool isRetry)
        {
            fFormatter = new BinaryFormatter();
            fFormatter.Binder = new SimpleObjectBinder();
            fFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
            //  Ensure the requests.adk file exists in the work directory
            String fileName = agent.WorkDir + Path.DirectorySeparatorChar + "requests.adk";
            FileInfo currentCacheFile = new FileInfo(fileName);
            if (!currentCacheFile.Exists)
            {
                if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
                {
                    Agent.Log.Debug("Creating SIF_Request ID cache: " + fileName);
                }
            }

            try
            {
                fFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read);
            }
            catch (Exception ioe)
            {
                throw new AdkException
                    ("Error opening or creating SIF_Request ID cache: " + ioe.Message, null, ioe);
            }

            //  Read the file contents into memory
            if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
            {
                Agent.Log.Debug("Reading SIF_Request ID cache: " + fileName);
            }

            // At startup, we pack the file by writing all of the active entries into
            // a second file and then replacing the original file with this copy
            String tmpName = null;
            FileStream tmp = null;

            try
            {
                tmpName = agent.WorkDir + Path.DirectorySeparatorChar + "requests.$dk";
                tmp = new FileStream(tmpName, FileMode.OpenOrCreate, FileAccess.Write);
                tmp.SetLength(0);

                int days = 90;
                String str = agent.Properties["adkglobal.requestCache.age"];
                if (str != null)
                {
                    try
                    {
                        days = Int32.Parse(str);
                    }
                    catch (Exception ex)
                    {
                        Agent.Log.Warn
                            (
                            "Error parsing property 'adkglobal.requestCache.age', default of 90 days will be used: " +
                            ex.Message, ex);
                    }
                }

                DateTime maxAge = DateTime.Now.Subtract(TimeSpan.FromDays(days));
                RequestCacheFileEntry next = null;
                while ((next = Read(fFile, false)) != null)
                {
                    if (next.IsActive && next.RequestTime > maxAge)
                    {
                        Store(tmp, next);
                    }
                }

                tmp.Close();
                fFile.Close();

                //
                //  Overwrite the requests.adk file with the temporary, then
                //  delete the temporary.
                //
                //if ((currentCacheFile.Attributes & FileAttributes.ReadOnly) != 0)
                //{
                //   currentCacheFile.Attributes = FileAttributes.Normal;
                //}
                currentCacheFile.Delete();
                File.Move(tmpName, fileName);

                try
                {
                    fFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
                }
                catch (Exception fnfe)
                {
                    throw new AdkException
                        ("Error opening or creating SIF_Request ID cache: " + fnfe, null, fnfe);
                }

                if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
                {
                    Agent.Log.Debug
                        ("Read " + fCache.Count + " pending SIF_Request IDs from cache");
                }
            }
            catch (Exception ioe)
            {
                //  Make sure the files are closed
                if (tmp != null)
                {
                    try
                    {
                        tmp.Close();
                    }
                    catch (Exception ex)
                    {
                        Agent.Log.WarnFormat("Exception thrown while closing FileStream: {0}", ex);
                    }

                }
                if (fFile != null)
                {
                    try
                    {
                        fFile.Close();
                    }
                    catch (Exception ex)
                    {
                        Agent.Log.WarnFormat("Exception thrown while closing file: {0}", ex);
                    }

                }
                if ((currentCacheFile.Attributes & FileAttributes.ReadOnly) != 0)
                {
                    throw new AdkException
                          ("Error opening or creating SIF_Request ID cache: " + ioe, null, ioe);

                }

                if (isRetry)
                {
                    // We've already tried reinitializing. rethrow
                    if (ioe is AdkException)
                    {
                        throw;
                    }
                    else
                    {
                        throw new AdkException
                            ("Error opening or creating SIF_Request ID cache: " + ioe, null, ioe);
                    }
                }
                else
                {
                    Agent.Log.Warn
                        ("Could not read SIF_Request ID cache (will start with fresh cache): " +
                          ioe);

                    //
                    //  Delete the files and re-initialize from scratch. We don't
                    //  want a file error here to prevent the agent from running, so
                    //  no exception is thrown to the caller.
                    //
                    File.Delete(fileName);
                    File.Delete(tmpName);
                    Initialize(agent, true);
                }
            }
        }

        /// <summary>  Closes the RequestCache</summary>
        public override void Close()
        {
            lock (this)
            {
                base.Close();
                try
                {
                    if (fFile != null)
                    {
                        fFile.Close();
                    }
                }
                catch (Exception ioe)
                {
                    throw new AdkException
                        ("Error closing SIF_Request ID cache: " + ioe, null, ioe);
                }
            }
        }
        /// <summary>  Return the count of active requests in the cache</summary>
        public override int ActiveRequestCount
        {
            get { return fCache.Count; }
        }

        /// <summary>  Store the request MsgId and associated SIF Data Object type in the cache</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IRequestInfo StoreRequestInfo(SIF_Request request,
                                                       Query q,
                                                       IZone zone)
        {
            // validate that the userdata supplied with the query is serializable
            if (q.UserData != null)
            {
                Type userDataType = q.UserData.GetType();
                if (!userDataType.IsSerializable)
                {
                    throw new ArgumentException
                        ("Query.UserData contains " + userDataType.AssemblyQualifiedName +
                          " which is not serializable");
                }
            }

            try
            {
                RequestCacheFileEntry entry = new RequestCacheFileEntry(true);
                entry.SetObjectType(request.SIF_Query.SIF_QueryObject.ObjectName);
                entry.SetMessageId(request.MsgId);
                entry.SetUserData(q.UserData);
                Store(fFile, entry);
                return entry;
            }
            catch (Exception thr)
            {
                throw new AdkException
                    ("Error writing to SIF_Request ID cache (MsgId: " + request.MsgId + ") " + thr,
                      zone, thr);
            }
        }

        private void Store(FileStream outStream,
                            RequestCacheFileEntry entry)
        {
            Boolean success;
            //cStore serialized Request in 2 parts
            //.NET framework returns null if Error occurs during
            //deserialization. 
            //In our case, when State error occurs, we'd still 
            //like to get the requestCacheFileEntry back
            //so I'm splitting the serialization into 2 parts:
            //State  and  requestCacheFileEntry
            //string l
            //
            entry.SetRequestTime(DateTime.Now);



            //store RequestFileCacheEntry to memory in HashTable
            fCache[entry.MessageId] = entry;


            // ******************************************************************
            //                                                          *********
            //             Serialize RequestCacheFile                   *********
            //                                                          *********
            outStream.Seek(0, SeekOrigin.End);
            entry.Location = outStream.Position;
            byte isActive = entry.IsActive ? (byte)1 : (byte)0;
            outStream.WriteByte(isActive);
            outStream.Flush();


            success = WriteNext(outStream, entry);
            if (success == false)
            {
                outStream.SetLength(entry.Location);
                return;
            }

            // ******************************************************************
            //                                                          *********
            //             Serialize State Information                  *********
            //                                                          *********
            // object l_objState = entry.State;
            WriteNext(outStream, entry.State);

        }

        /// <summary>
        /// Reads the next RequestCachFile Request from the FileStream
        /// and advances the Position of the filestream .
        /// The first 4 bytes return the length of the serialized object
        /// The remaining Bytes holds the serialized object bytes.
        /// </summary>
        /// <param name="outStream">The stream to read objects out of</param>
        private object ReadNext(FileStream outStream)
        {
            byte[] rawLength = new byte[4];
            outStream.Read(rawLength, 0, 4);

            int objectLength = BitConverter.ToInt32(rawLength, 0);
            if (objectLength > 0)
            {
                byte[] serializedObject = new byte[objectLength];
                outStream.Read(serializedObject, 0, objectLength);
                MemoryStream ms = new MemoryStream(serializedObject, false);
                return fFormatter.Deserialize( ms );
            }
            return null;
        }


        /// <summary>
        /// Writes the next RequestCachFile Request to the FileStream
        /// and advances the Position of the filestream .
        /// The first 4 bytes return the length of the serialized object
        /// The remaining Bytes holds the object bytes.
        /// </summary>
        /// <param name="outStream">The stream being serialized</param>
        /// <param name="serializedObject">The object being serialized</param>
        private bool WriteNext(FileStream outStream, object serializedObject)
        {

            byte[] objectRawLength = new byte[4];
            outStream.Seek(0, SeekOrigin.End);

            //Make placeholder for serialized object length, write to fs
            outStream.Write(objectRawLength, 0, 4);
            
            if (serializedObject == null)
            {
                return true;
            }
            else
            {
                
                //Record starting position of stream where serialized object begins
                long startPosition = outStream.Position;

                try
                {
                    //serialize and write object to fs
                    fFormatter.Serialize(outStream, serializedObject);
                }
                catch (Exception ex)
                {
                    Agent.Log.Error
                        ("Exception occurred while Writing RequestInfo State object to RequestCacheFile: " +
                         ex.Message, ex);
                    outStream.Seek(startPosition, SeekOrigin.Begin);
                    outStream.SetLength( startPosition );
                    return false;

                }
                outStream.Flush();

                // Write the serialized object's length to fs at placeholder position from above
                int objLength = (int) (outStream.Position - startPosition);
                objectRawLength = BitConverter.GetBytes(objLength);
                outStream.Seek(startPosition - 4, SeekOrigin.Begin);
                outStream.Write(objectRawLength, 0, 4);
                outStream.Flush();
                // Advance to the end of the stream
                outStream.Seek(0, SeekOrigin.End);
                return true;
            }
        }


        /// <summary>
        /// Reads the next RequestCacheFileEntry from the FileStream
        /// </summary>
        /// <param name="outStream">The filestream to read the entry from</param>
        /// <param name="readInactiveData">If TRUE, all Entries properties will be deserialized, even if inactive. If
        /// False, only active Entries will be deserialized and inactive Entries will be returned as an Entry
        /// with the "IsActive" property set to false </param>
        /// <returns>The next entry or null if at the end of the stream</returns>
        private RequestCacheFileEntry Read(FileStream outStream,
                                            bool readInactiveData)
        {

            int active = outStream.ReadByte();
            if (active == -1)
            {
                // We are at the end of the FileStream
                return null;
            }
            else
            {

                if (active == 0 && !readInactiveData)
                {
                    // Move the stream forward and skip over the serialized entries
                    ReadNext(outStream);
                    ReadNext(outStream);
                    // above objects are throw-away variables.
                    //They are only used to advance the filestream
                    return new RequestCacheFileEntry(false);
                }
                else
                {
                    RequestCacheFileEntry returnValue;
                    try
                    {
                        //************************************************************
                        //      deserialize RequestCacheFileEntry                *****
                        returnValue = (RequestCacheFileEntry)ReadNext(outStream);
                    }
                    catch (Exception ex)
                    {
                        Agent.Log.Warn
                            ("Error Deserializing Request Cache Info: " + ex.Message, ex);
                        return new RequestCacheFileEntry(false);
                    }

                    //************************************************************
                    //      deserialize the State object                     *****
                    try
                    {
                        Object state = ReadNext(outStream);
                        returnValue.State = state;
                    }
                    catch (Exception ex)
                    {
                        Agent.Log.Warn
                        ("Error Deserializing Request Cache State Info: " + ex.Message, ex);

                    }
                    returnValue.SetIsActive(active == 1);
                    return returnValue;


                }
            }
        }

        /// <summary>  Lookup the SIF Data Object type of a pending request given its MsgId,
        /// then remove the entry from the cache. To lookup an entry without removing
        /// it, call the lookupRequestObjectType method.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IRequestInfo GetRequestInfo(string msgId,
                                                     IZone zone)
        {
            return Lookup(msgId, zone, true);
        }


        /// <summary>
        ///  Lookup the SIF Data Object type of a pending request given its MsgId
        /// </summary>
        /// <param name="msgId">The msgId of the request</param>
        /// <param name="zone">The zone associated with the request</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override IRequestInfo LookupRequestInfo(string msgId,
                                                        IZone zone)
        {
            return Lookup(msgId, zone, false);
        }

        /// <summary>
        /// Looks up the specified entry in the cache.
        /// </summary>
        /// <param name="msgId">The message id to lookup</param>
        /// <param name="zone">The zone associated with the message</param>
        /// <param name="remove">If TRUE, the entry will be removed from the cache</param>
        /// <returns>The entry associated with the specified message ID or NULL if no entry was found</returns>
        private IRequestInfo Lookup(string msgId,
                                     IZone zone,
                                       bool remove)
        {
            RequestCacheFileEntry e = (RequestCacheFileEntry)fCache[msgId];
            if (e == null)
            {
                return null;
            }

            if (remove)
            {
                fCache.Remove(msgId);
                try
                {
                    fFile.Seek(e.Location, SeekOrigin.Begin);
                    fFile.WriteByte(0);
                }
                catch (Exception ioe)
                {
                    throw new AdkException
                        ("Error removing entry from SIF_Request ID cache: " + ioe.Message, zone,
                          null);
                }
            }

            return e;
        }

        private class SimpleObjectBinder : SerializationBinder
        {
            /// <summary>
            /// This method uses a simple algorithm to ensure that objects are able to be deserialized
            /// across major versions in .Net. The default serialization binder will throw an exception
            /// if the object being deserialized is either 
            /// 
            /// 1) strong-named and any version part is different or
            /// 
            /// 2) not strong-named, but differs by major or minor version parts.
            /// </summary>
            /// <param name="assemblyName">The assembly name of the object</param>
            /// <param name="typeName">The type name of the object</param>
            /// <returns>The Type of the object</returns>
            public override Type BindToType(
                string assemblyName,
                string typeName)
            {
                int assemblyNamePart = assemblyName.IndexOf(',');
                if (assemblyNamePart == -1)
                {
                    assemblyNamePart = assemblyName.Length;
                }
                string className =
                    string.Format
                        ("{0},{1}", typeName, assemblyName.Substring(0, assemblyNamePart));
                return Type.GetType(className);
            }
        }
    }
}
