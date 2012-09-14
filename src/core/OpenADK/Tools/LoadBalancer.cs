//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

namespace OpenADK.Library.Tools
{
    /// <summary>  A LoadBalancer manages a free pool of <i>Baton</i> objects representing the
    /// right of a thread to perform a resource-intensive task.</summary>
    /// <remarks>
    /// <para>
    ///  For example, you
    /// could create a LoadBalancer that represents the task "query all students"
    /// and assign it an initial pool of 5 Batons, meaning at most 5 threads will be
    /// able to carry out this task at once. A thread must check out a Baton in
    /// order to perform the task, and must release it back to the LoadBalancer
    /// when finished.
    /// </para>
    /// <para>
    /// Refer to the Baton class for a description of how to use the LoadBalancer
    /// and Baton classes and the LoadBalancerListener interface. These classes can
    /// be used to introduce internal load balancing into an agent to significantly
    /// improve scalability when connecting to tens or hundreds of zones
    /// concurrently.
    /// </para>
    /// </remarks>
    /// <author> Data Solutions
    /// </author>
    /// <since>  ADK 1.0
    /// </since>
    public class LoadBalancer
    {
        /// <summary>  Gets the current load (the number of Batons in use)</summary>
        public virtual int Load
        {
            get
            {
                lock ( fPool.SyncRoot ) {
                    return fSize - fPool.Count;
                }
            }
        }

        /// <summary>  Gets the total number of Batons</summary>
        public virtual int TotalBatons
        {
            get { return fSize; }
        }

        /// <summary>  Gets the number of Batons</summary>
        public virtual int FreeBatons
        {
            get
            {
                lock ( fPool.SyncRoot ) {
                    return fPool.Count;
                }
            }
        }

        public static bool TRACE = false;

        /// <summary>  LoadBalancer ID</summary>
        protected internal Object fID;

        /// <summary>  Pool of batons</summary>
        protected internal Stack fPool = new Stack();

        /// <summary>  Maximum number of Batons</summary>
        protected internal int fSize;

        /// <summary>  checkoutBaton timeout value</summary>
        protected internal long fTimeout;

        /// <summary>  Global dictionary of LoadBalancers</summary>
        /// <seealso cref="Define">
        /// </seealso>
        protected internal static IDictionary sDict = new HybridDictionary();

        /// <summary>  Flagged true when the pool is emptied</summary>
        protected internal bool fEmptied = false;

        /// <summary>  Listeners</summary>
        protected internal ArrayList fListeners = null;


        /// <summary>  Constructs a LoadBalancer to represent a specific logical task.
        /// 
        /// </summary>
        /// <param name="id">A unique arbitrary ID that the agent will use to request this
        /// LoadBalancer (e.g. "Request_StudentPersonal")
        /// </param>
        /// <param name="batons">The number of Batons that will be available to threads
        /// </param>
        /// <param name="timeout">The timeout period (in milliseconds) applied to the
        /// <c>checkoutBaton</c> method. The timeout period should be less
        /// than the HTTP or other transport timeout period so that the connection
        /// to the ZIS does not timeout before the load balancer does.
        /// </param>
        public LoadBalancer( Object id,
                             int batons,
                             long timeout )
        {
            if ( id == null ) {
                throw new ArgumentException( "id cannot be null" );
            }

            fID = id;
            fTimeout = timeout;
            fSize = batons;

            //  Create pool of batons
            int _batons = Math.Max( 1, batons );
            for ( int i = 0; i < _batons; i++ ) {
                fPool.Push( new Baton() );
            }
        }

        /// <summary>  Define a LoadBalancer that may be subsequently returned by the <c>lookup</c> method.</summary>
        /// <param name="balancer">A LoadBalancer instance
        /// </param>
        public static void Define( LoadBalancer balancer )
        {
            lock ( sDict.SyncRoot ) {
                sDict[balancer.fID] = balancer;
            }
        }

        /// <summary>  Lookup a LoadBalancer that was previously defined by the <c>define</c> method.</summary>
        /// <param name="id">The ID of the LoadBalancer to obtain
        /// </param>
        /// <returns> The LoadBalancer or null if no LoadBalancer with this id has been
        /// previously defined by the <c>define</c> method
        /// </returns>
        public static LoadBalancer lookup( Object id )
        {
            lock ( sDict.SyncRoot ) {
                return (LoadBalancer) sDict[id];
            }
        }

        /// <summary>  Check-out a Baton.</summary>
        public virtual Baton CheckoutBaton()
        {
            Baton b = null;

            lock ( fPool.SyncRoot ) {
                if ( fPool.Count == 0 ) {
                    try {
                        //  Wait for an instance to become available
                        if ( TRACE ) {
                            Console.Out.WriteLine
                                ( "Waiting for baton to become available (" +
                                  Thread.CurrentThread.Name + ")" );
                        }
                        Monitor.Wait( fPool.SyncRoot, TimeSpan.FromMilliseconds( fTimeout ) );
                        if ( TRACE ) {
                            Console.Out.WriteLine
                                ( "Done waiting for baton to become available (" +
                                  Thread.CurrentThread.Name + ")" );
                        }
                    }
                    catch ( ThreadInterruptedException ) {}
                }

                if ( fPool.Count > 0 ) {
                    b = (Baton) fPool.Pop();
                }
                if ( TRACE ) {
                    Console.Out.WriteLine
                        ( b == null
                              ? "No baton available"
                              : "Got a baton (there are " + fPool.Count + " left)" );
                }
                if ( fPool.Count == 0 ) {
                    fEmptied = true;
                }
            }

            return b;
        }

        /// <summary>  Check-in a Baton.</summary>
        public virtual void CheckinBaton( Baton baton )
        {
            if ( baton != null ) {
                lock ( fPool.SyncRoot ) {
                    fPool.Push( baton );
                    if ( TRACE ) {
                        Console.Out.WriteLine( "Baton checked in; there are now " + fPool.Count );
                    }

                    if ( fEmptied && fPool.Count >= (fSize == 1 ? 1 : 2) ) {
                        if ( TRACE ) {
                            Console.Out.WriteLine
                                ( "Notifying listeners that batons are now available" );
                        }

                        //  Notify all listeners that Batons are once again available
                        fEmptied = false;
                        if ( fListeners != null ) {
                            lock ( fListeners.SyncRoot ) {
                                for ( int i = 0; i < fListeners.Count; i++ ) {
                                    ILoadBalancerListener l = (ILoadBalancerListener) fListeners[i];
                                    l.OnBatonsAvailable( this );
                                }
                            }
                        }
                    }

                    try {
                        //  Notify all waiting threads that a Baton is available
                        if ( TRACE ) {
                            Console.Out.WriteLine
                                ( "Notifying threads that baton is returned to free pool" );
                        }
                        Monitor.PulseAll( fPool );
                    }
                    catch ( Exception ) {}
                }
            }
        }

        /// <summary>  Register a LoadBalancerListener with this LoadBalancer. The listener will
        /// be called when the free pool is empty and subsequently contains at least
        /// two Batons (or one Baton if this LoadBalancer was defined to have a pool
        /// size of one).
        /// </summary>
        public virtual void AddLoadBalancerListener( ILoadBalancerListener listener )
        {
            if ( fListeners == null ) {
                fListeners = new ArrayList();
            }

            fListeners.Add( listener );
        }

        /// <summary>  Remove a LoadBalancerListener previously registered with the <c>
        /// addLoadBalancerListener</c> method
        /// </summary>
        public virtual void removeLoadBalancerListener( ILoadBalancerListener listener )
        {
            if ( fListeners != null ) {
                if ( fListeners.Contains( listener ) ) {
                    fListeners.Remove( listener );
                }
            }
        }
    }
}
