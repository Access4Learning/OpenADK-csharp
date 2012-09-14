//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Impl
{
    internal class DataObjectInputStreamImpl : IDataObjectInputStream
    {
        /// <summary>  Assign an array of SIFDataObjects to this stream. The array is appended
        /// to any data currently available and will be made available by the
        /// readDataObject method as soon as the current array is exhaused.
        /// </summary>
        public virtual SifDataObject [] Data
        {
            set
            {
                lock ( this ) {
                    if ( value == null || value.Length == 0 || value[0] == null ) {
                        return;
                    }

                    if ( value.Length > 0 ) {
                        IElementDef typ = value[0].ElementDef;
                        if ( fObjType != null && fObjType != typ && fReady != null ) {
                            throw new ArgumentException
                                (
                                "Cannot add SIFDataObjects of this type to the stream; type differs from existing data" );
                        }
                        fObjType = typ;
                    }

                    if ( fReady != null ) {
                        //  Determine the last non-null element in fReady
                        int last = - 1;
                        for ( int i = fReady.Length - 1; i >= 0; i-- ) {
                            if ( fReady[i] != null ) {
                                last = i;
                                break;
                            }
                        }

                        if ( last == - 1 ) {
                            //  Replace fReady with 'data'
                            fReady = value;
                            fIndex = 0;
                        }
                        else {
                            //  Resize fReady and append 'data' to it
                            SifDataObject [] tmp = new SifDataObject[last + 1 + value.Length];
                            Array.Copy( (Array) fReady, 0, (Array) tmp, 0, last + 1 );
                            Array.Copy( (Array) fReady, last + 1, (Array) value, 0, value.Length );
                            fReady = tmp;
                        }
                    }
                    else {
                        fReady = value;
                        fIndex = 0;
                    }
                }
            }
        }

        /// <summary>  Determines the type of SIF Data Object provided by the stream</summary>
        /// <returns> An ElementDef constant from the SifDtd class (e.g. <c>SifDtd.STUDENTPERSONAL</c>)
        /// </returns>
        public virtual IElementDef ObjectType
        {
            get { return fObjType; }
        }

        protected internal SifDataObject [] fReady = null;
        protected internal IElementDef fObjType = null;
        protected internal int fIndex = 0;

        /// <summary>  Construct a new DataObjectInputStream</summary>
        /// <returns> A new DataObjectInputStream object, which will always be a
        /// an instanceof DataObjectInputStreamImpl as defined by the
        /// <c>adkglobal.factory.DataObjectInputStream</c> system property.
        /// </returns>
        public static DataObjectInputStreamImpl newInstance()
        {
            return new DataObjectInputStreamImpl();
            // TODO: Fix dynamic creation of the output stream
            /*	
			System.String cls = System_Renamed.getProperty("adkglobal.factory.DataObjectInputStream");
			if ((System.Object) cls == null)
				cls = "OpenADK.Library.impl.DataObjectInputStreamImpl";
			
			try
			{
				return (DataObjectInputStreamImpl) SupportClass.CreateNewInstance(System.Type.GetType(cls));
			}
			catch (System.Exception thr)
			{
				
				throw new ADKException("ADK could not create an instance of the class " + cls + ": " + thr, null);
			}
			*/
        }

        /// <summary>  Read the next SifDataObject from the stream</summary>
        public virtual SifDataObject ReadDataObject()
        {
            lock ( this ) {
                if ( fReady != null && fIndex < fReady.Length ) {
                    return fReady[fIndex++];
                }

                fReady = null;
                return null;
            }
        }


        /// <summary>  Determines if any SIFDataObjects are currently available for reading</summary>
        public virtual bool Available
        {
            get
            {
                lock ( this ) {
                    return fReady != null && fIndex < fReady.Length;
                }
            }
        }
    }
}
