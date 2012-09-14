//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Web
{
    /// <summary>Called when a socket error occurs </summary>
    public delegate void AdkSocketErrorHandler( AdkSocketConnection socket,
                                                Exception exception );
}
