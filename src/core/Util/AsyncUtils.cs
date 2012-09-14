//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Threading;

namespace OpenADK.Util
{
    /// <summary>
    /// This class allows you to safely invoke a delegate asynchronously, similarly to calling
    /// BeginInvoke(), without ever having to call EndInvoke(). Simply call AsyncHelper.QueueTaskToThreadPool()
    /// </summary>
    /// <remarks>
    /// <para>
    /// Starting with the 1.1 release of the .NET Framework, the SDK docs
    /// now carry a caution that mandates calling EndInvoke on delegates
    /// you've called BeginInvoke on in order to avoid potential leaks.
    /// This means you cannot simply "fire-and-forget" a call to BeginInvoke
    /// without the risk of running the risk of causing problems.
    /// </para>
    /// <example>
    /// For example, assuming a delegate defined as follows:
    /// <code>
    /// delegate void CalcAndDisplaySumDelegate( int a, int b );
    /// </code>
    /// Instead of doing this to fire-and-forget an async call to some
    /// target method:
    /// <code>
    /// CalcAndDisplaySumDelegate d = new CalcAndDisplaySumDelegate(someCalc.Add);
    /// d.BeginInvoke(2, 3, null);
    /// </code>
    /// You would instead do this:
    /// <code>
    /// CalcAndDisplaySumDelegate d = new CalcAndDisplaySumDelegate(someCalc.Add);
    /// AsyncHelper.QueueTaskToThreadPool(d, 2, 3);
    /// </code>
    /// </example>
    /// <para>
    /// This code was created from a sample by Mike Woodring( http://staff.develop.com/woodring )
    /// </para>
    /// </remarks>
    public class AsyncUtils
    {
        /// <summary>
        /// This method allows you to safely invoke a delegate asynchronously, similarly to calling
        /// BeginInvoke(), without ever having to call EndInvoke().
        /// </summary>
        /// <param name="d">The delegate to call</param>
        /// <param name="args">The arguments for the delegate (or null)</param>
        public static void QueueTaskToThreadPool( Delegate d,
                                                  params object [] args )
        {
            
            ThreadPool.QueueUserWorkItem( dynamicInvokeShim, new TargetInfo( d, args ) );
        }


        private static void DynamicInvokeShim( object o )
        {
            TargetInfo ti = (TargetInfo) o;
 
            ti.Target.DynamicInvoke( ti.Args );
        }

        private class TargetInfo
        {
            internal TargetInfo( Delegate d,
                                 object [] args )
            {
                Target = d;
                Args = args;
            }

            internal readonly Delegate Target;
            internal readonly object [] Args;
        }

        private static WaitCallback dynamicInvokeShim = new WaitCallback( DynamicInvokeShim );
    }
}
