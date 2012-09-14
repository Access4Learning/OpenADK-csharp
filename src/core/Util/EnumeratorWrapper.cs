//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;

namespace OpenADK.Util
{
    /// <summary>
    /// A base class for creating an enumerator that wraps around another enumerator
    /// </summary>
    /// <remarks>
    /// When wrapping another enumerator, there are a number of common tasks that this class
    /// takes care of, including disposing of it properly when dispose is called.
    /// </remarks>
    public abstract class EnumeratorWrapper : IEnumerator, IDisposable
    {
        protected EnumeratorWrapper( IEnumerator inWrappedEnumerator )
        {
            fWrappedEnumerator = inWrappedEnumerator;
        }

        #region IEnumerator

        public virtual bool MoveNext()
        {
            return fWrappedEnumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            fWrappedEnumerator.Reset();
        }

        public virtual object Current
        {
            get { return fWrappedEnumerator.Current; }
        }

        #endregion

        protected IEnumerator WrappedEnumerator
        {
            get { return fWrappedEnumerator; }
        }

        #region IDisposable

        private void Dispose( bool disposing )
        {
            if ( fWrappedEnumerator != null ) {
                IDisposable aObject = fWrappedEnumerator as IDisposable;
                if ( aObject != null ) {
                    aObject.Dispose();
                }
                fWrappedEnumerator = null;
            }
//			if( disposing )
//			{
//				GC.SuppressFinalize( this );
//			}
        }

        public virtual void Dispose()
        {
            Dispose( true );
        }

        #endregion

        private IEnumerator fWrappedEnumerator;
    }
}
