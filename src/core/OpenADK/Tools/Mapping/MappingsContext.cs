//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Collections.Generic;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Tools.Mapping
{
    public class MappingsContext
    {
        private SifXPathContext fRootContext;
        private SifVersion fSIFVersion;
        private IElementDef fElementDef;
        private MappingDirection fDirection;
        private IValueBuilder fValueBuilder;
        private List<FieldMapping> fFieldMappings = new List<FieldMapping>();
        private Mappings fMappings;
        private ObjectMapping fObjectMappings;


        /**
	 * @param mappings
	 * @param direction
	 * @param version
	 * @param elementType
	 */

        private MappingsContext(
            Mappings mappings,
            MappingDirection direction,
            SifVersion version,
            IElementDef elementType)
        {
            fMappings = mappings;
            fDirection = direction;
            fSIFVersion = version;
            fElementDef = elementType;
        }

        /**
	 * Creates a MappingsContext instance to handle a set of mappings operations using
	 * the same parameters
	 * @param m The mappings instance to use
	 * @param direction The mappings direction
	 * @param version The version of SIF to use for evaluating rule filters on field mappings
	 * @param elementDef The ElementDef representing the object type being mapped
	 * @return A new MappingsContext, initialized to map using the specified parameters
	 */

        public static MappingsContext Create(Mappings m, MappingDirection direction, SifVersion version,
                                             IElementDef elementDef)
        {
            // Get the rules associated with the element type
            MappingsContext mc = new MappingsContext(m, direction, version, elementDef);
            ObjectMapping om = m.GetRules(elementDef.Name, true);
            mc.AddRules(om);
            return mc;
        }

        private void AddRules(ObjectMapping om)
        {
            // Get the rules associated with the element type
            fObjectMappings = om;
            if (om != null)
            {
                foreach (FieldMapping fm in om.GetRulesList(true))
                {
                    // addRule( FieldMapping ) will automatically filter out
                    // any rules that need to be filtered
                    AddRule(fm);
                }
            }
        }

        private SifXPathContext GetXPathContext(
            SifElement mappedElement, IFieldAdaptor adaptor)

        {
            lock (this)
            {
                if (!mappedElement.ElementDef.Name.Equals(fElementDef.Name))
                {
                    throw new AdkMappingException(
                        "Unable to use object for mapping. MappingsContext expected an object of type '" +
                        fElementDef.Name + "' but was '" + mappedElement.ElementDef.Name + "'.", null);
                }

                if (fRootContext == null)
                {
                    fRootContext = SifXPathContext.NewSIFContext(mappedElement, fSIFVersion);
                    if (adaptor is IXPathVariableLibrary)
                    {
                        fRootContext.AddVariables( "", (IXPathVariableLibrary)adaptor);
                    }
                }
                return SifXPathContext.NewSIFContext(fRootContext, mappedElement);
            }
        }

        /**
	 * Perform a mapping operation on the specified SIFElement. The mapping operation
	 * will be either inbound or outbound, depending on whether this class was returned
	 * from {@link Mappings#selectInbound(ElementDef, SIFVersion, String, String)} or
	 * {@link Mappings#selectOutbound(ElementDef, SIFVersion, String, String)}
	 * @param mappedElement The SIFElement to perform the mappings operation on
	 * @param adaptor The FieldAdaptor to use for getting or setting data
	 * @throws ADKMappingException
	 */

        public void Map(SifElement mappedElement, IFieldAdaptor adaptor)
        {
            SifXPathContext context = GetXPathContext(mappedElement, adaptor);
            if (fDirection == MappingDirection.Inbound)
            {
                fMappings.MapInbound(context, adaptor, mappedElement, fFieldMappings, fSIFVersion);
            }
            else if (fDirection == MappingDirection.Outbound)
            {
                fMappings.MapOutbound(context, adaptor, mappedElement, fFieldMappings, fValueBuilder, fSIFVersion);
            }
        }

        /**
	 * Evaluates the filters defined for this FieldMapping. If any of the filters
	 * evaluate to false, the FieldMapping is not added
	 * 
	 * @param fieldMapping The FieldMapping to add
	 * @return True if the FieldMapping was added. Otherwise false
	 */

        private bool AddRule(FieldMapping fieldMapping)
        {
            MappingsFilter filt = fieldMapping.Filter;
            //	Filter out this rule?
            if (filt != null)
            {
                if (!filt.EvalDirection(fDirection) ||
                    !filt.EvalVersion(fSIFVersion))
                    return false;
            }

            fFieldMappings.Add(fieldMapping);
            return true;
        }


        /**
	 * Gets the MappingsDirection being used for this Mappings Context
	 * @return A MappingsDirection value (INBOUND or OUTBOUND)
	 */

        public MappingDirection Direction
        {
            get { return fDirection; }
        }

        /**
	 * Sets the ValueBuilder instance to use for mapping operations. 
	 * @param functions A class implementing the ValueBuilder interface
	 */

        public void SetValueBuilder(IValueBuilder functions)
        {
            fValueBuilder = functions;
        }


        /**
	 * Gets the ObjectMapping instance that this MappingsContext was initialized with
	 * @return The ObjectMapping being used for this MappingsContext
	 */

        public ObjectMapping ObjectMappings
        {
            get { return fObjectMappings; }
        }

        /**
	 * Returns an unmodifiable collection of the FieldMappings defined
	 * in this mapping context
	 * @return an unmodifiable collection of FieldMappings
	 */

        public IList<FieldMapping> FieldMappings
        {
            get { return fFieldMappings.AsReadOnly(); }
        }


        /**
	 * Returns the ElementDef that this MappingsContext is initialized to
	 * map values for
	 * @return The ElementDef that this MappingsContext is mapping data for
	 */

        public IElementDef ObjectDef
        {
            get { return fElementDef; }
        }

        /**
	 * Returns the Mappings object that this context is using to perform
	 * Mapping operations.
	 * @return a Mappings instance
	 */

        public Mappings Mappings
        {
            get { return fMappings; }
        }
    }
}
