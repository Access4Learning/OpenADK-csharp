//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using OpenADK.Library.Impl.Surrogates;

namespace OpenADK.Library.Tools.XPath
{
    internal abstract class SimpleFieldPointer : AdkElementPointer
    {
        /// <summary>
        /// Creates a SimpleFieldPointer
        /// </summary>
        /// <param name="parentPointer">The parent of this pointer</param>
        /// <param name="element">The element being wrapped</param>
        /// <param name="version">The SifVersion to use for resolving XPaths</param>
        /// set of fields</param>
        internal SimpleFieldPointer(
            INodePointer parentPointer,
            Element element,
            SifVersion version )
            : base( parentPointer, element, version )
        {
        }

        /// <summary>
        /// Creates a new NodePointer representing the SimpleField
        /// </summary>
        /// <param name="pointerParent"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static INodePointer Create(
            SifElementPointer pointerParent, SimpleField field )
        {
            SifVersion version = pointerParent.Version;

            if ( pointerParent.IsLegacyVersion )
            {
                IElementDef fieldDef = field.ElementDef;
                IRenderSurrogate rs = fieldDef.GetVersionInfo( version ).GetSurrogate();
                if ( rs != null )
                {
                    return rs.CreateNodePointer( pointerParent, field, version );
                }
            }

            if ( field.ElementDef.IsAttribute( version ) )
            {
                return new SimpleFieldAttributePointer( pointerParent, field, version );
            }
            else
            {
                return new SimpleFieldElementPointer( pointerParent, field, version );
            }
        }


        public override void SetValue( object rawValue )
        {
            SifElementPointer immediateParent = (SifElementPointer) Parent;
            SifElement parentElement = immediateParent.Element as SifElement;
            SifSimpleType sst = GetSIFSimpleTypeValue( fElementDef, rawValue );
            parentElement.SetField( fElementDef, sst );
        }
    }
}
