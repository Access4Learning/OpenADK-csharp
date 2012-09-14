//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenADK.Util
{
    public sealed class AdkConsoleWait
    {
        /// <summary>
        /// This method waits and does not return until the Console Window is messaged to close by windows
        /// </summary>
        /// <returns></returns>
        public AdkConsoleEvent WaitForExit()
        {
            fLifeCycle = new ManualResetEvent( false );
            using ( AdkConsoleEventWatcher watcher = new AdkConsoleEventWatcher() ) {
                watcher.ControlEvent += new AdkControlEventHandler( watcher_ControlEvent );
                fLifeCycle.WaitOne();
                return fReceivedEvent;
            }
        }

        public void Exit( AdkConsoleEvent c )
        {
            if ( !fIsExiting ) {
                fIsExiting = true;
                fReceivedEvent = c;
                if ( Exiting != null ) {
                    Exiting( this, new AdkConsoleEventArgs( c ) );
                }
                if ( fLifeCycle != null ) {
                    fLifeCycle.Set();
                }
            }
        }

        public bool IsExiting
        {
            get { return fIsExiting; }
        }


        private void watcher_ControlEvent( object src,
                                           AdkConsoleEventArgs args )
        {
            Exit( args.Event );
        }

        public AdkConsoleEvent ExitCode
        {
            get { return fReceivedEvent; }
        }

        public event AdkControlEventHandler Exiting;

        private AdkConsoleEvent fReceivedEvent;
        private bool fIsExiting;
        private ManualResetEvent fLifeCycle;
    }


    /// <summary>
    /// The event that occurred (From wincom.h)
    /// </summary>
    public enum AdkConsoleEvent
    {
        /// <summary>
        /// CTRL-C was pressed
        /// </summary>
        CTRL_C = 0,
        /// <summary>
        /// CTRL-BREAK was pressed
        /// </summary>
        CTRL_BREAK = 1,
        /// <summary>
        /// The windows was messaged to close
        /// </summary>
        CTRL_CLOSE = 2,
        /// <summary>
        /// The user is logging off
        /// </summary>
        CTRL_LOGOFF = 5,
        /// <summary>
        /// The system is shutting down
        /// </summary>
        CTRL_SHUTDOWN = 6
    }

    /// <summary>
    /// Handler to be called when a console event occurs.
    /// </summary>
    public delegate void AdkControlEventHandler( object src,
                                                 AdkConsoleEventArgs args );

    public sealed class AdkConsoleEventArgs : EventArgs
    {
        public AdkConsoleEventArgs( AdkConsoleEvent e )
        {
            fEvent = e;
        }

        public AdkConsoleEvent Event
        {
            get { return fEvent; }
        }

        private AdkConsoleEvent fEvent;
    }


    /// <summary>
    /// Summary description for ConsoleEventHandler.
    /// </summary>
    public sealed class AdkConsoleEventWatcher : IDisposable
    {
        /// <summary>
        /// Event fired when a console event occurs
        /// </summary>
        public event AdkControlEventHandler ControlEvent;

        private Win32ConsoleCtrlHandler eventHandler;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public AdkConsoleEventWatcher()
        {
            // save this to a private var so the GC doesn't collect it...
            eventHandler = new Win32ConsoleCtrlHandler( Handler );
            SetConsoleCtrlHandler( eventHandler, true );
        }

        ~AdkConsoleEventWatcher()
        {
            Dispose( false );
        }

        public void Dispose()
        {
            Dispose( true );
        }

        private void Dispose( bool disposing )
        {
            if ( eventHandler != null ) {
                SetConsoleCtrlHandler( eventHandler, false );
                eventHandler = null;
            }
            if ( disposing ) {
                GC.SuppressFinalize( this );
            }
        }

        private delegate void Win32ConsoleCtrlHandler( AdkConsoleEvent e );

        private void Handler( AdkConsoleEvent consoleEvent )
        {
            if ( ControlEvent != null ) {
                try {
                    ControlEvent( this, new AdkConsoleEventArgs( consoleEvent ) );
                }
                catch {}
            }
        }

        [DllImport( "kernel32.dll" )]
        private static extern bool SetConsoleCtrlHandler( Win32ConsoleCtrlHandler e,
                                                          bool add );
    }
}
