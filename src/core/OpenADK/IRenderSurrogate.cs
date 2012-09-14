//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    public interface IRenderSurrogateToDelete
    {
        void Render( SifWriter writer,
                     SifElement element,
                     SifFormatter formatter );
    }
}
