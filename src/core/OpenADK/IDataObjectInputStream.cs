//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>DataObjectInputStream is supplied to message handlers to allow agents to
    /// stream an arbitrarily large set of SIFDataObjects from SIF_Response 
    /// and SIF_Event messages.<</summary>
    /// <remarks>
    /// To use DataObjectInputStream, construct a while loop that calls 
    /// <see cref="#Available"/> to determine if more objects are available from the 
    /// stream. Within the loop, call <see cref="#ReadDataObject"/> to obtain the next 
    /// SifDataObject instance from the stream. Note all SifDataObjects in the stream 
    /// are of the same type. To determine the type, use the <see cref="#ObjectType"/>
    /// property to retrieve an ElementDef constant from the <see cref="OpenADK.Library.SifDtd"/> class
    /// </remarks>
    /// <example>For Example
    /// <code language="C#">
    /// if( myStream.ObjectType == SIFDTD.STUDENTPERSONAL )
    /// {
    /// 	while( myStream.Available ) 
    /// 	{
    /// 		StudentPersonal sp = ( StudentPersonal )myStream.ReadDataObject();
    /// 			...
    /// 	}
    /// }
    /// </code>
    /// </example>
    public interface IDataObjectInputStream
    {
        /// <summary>  Determines the type of SIF Data Object provided by the stream</summary>
        /// <returns> An ElementDef constant from the SifDtd class (e.g. <c>SifDtd.STUDENTPERSONAL</c>)</returns>
        IElementDef ObjectType { get; }

        /// <summary>  Read the next SifDataObject from the stream</summary>
        SifDataObject ReadDataObject();

        /// <summary>  Determines if any SIFDataObjects are currently available for reading</summary>
        bool Available { get; }
    }
}

// Synchronized with DataObjectInputStream.java Branch Library-ADK-1.5.0 version 2
