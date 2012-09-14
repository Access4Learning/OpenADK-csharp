//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;
using OpenADK.Library.Tools.XPath.Compiler;
using OpenADK.Library.Tools.XPath.Functions;

namespace OpenADK.Library.Tools.XPath
{
    public class SifXPathContext : IXPathNavigable
    {
        private SifElement fContextElement;
        private SifElementPointer fContextPointer;
        private SifXsltContext fContext;
        private SifXPathNavigator fDefaultNavigator;


        public SifElement ContextElement
        {
            get { return fContextElement; }
        }


        /// <summary>
        /// Creates a new SifXPathContext instance to use for traversing the
        /// specified SIF Data Object
        /// </summary>
        /// <param name="sdo">The SIF Data Object or SIFElement to traverse</param>
        /// <returns>an instance of SifXPathContext</returns>
        public static SifXPathContext NewSIFContext( SifElement sdo )
        {
            return NewSIFContext( null, sdo );
        }


        /// <summary>
        /// Creates a new SifXPathContext instance to use for traversing the
        /// specified SIF Data Object
        /// </summary>
        /// <remarks>
        /// NOTE: The SIFDataObject.setSIFVersion(version) is automatically
        /// called and set to the target version.
        /// </remarks>
        /// <param name="sdo">The SIF Data Object or SIFElement to traverse</param>
        /// <returns>an instance of SifXPathContext</returns>
        /// <param name="version">The SIFVersion to use when traversing this object using XPath.</param>
        public static SifXPathContext NewSIFContext( SifElement sdo, SifVersion version )
        {
            sdo.SifVersion = version;
            SifXPathContext context = NewSIFContext( sdo );
            return context;
        }


        /// <summary>
        /// Creates a new SifXPathContext instance to use for traversing the
        /// specified SIF Data Object
        /// </summary>
        /// <remarks>
        /// Contexts that are created from other contexts automatically inherit all
        /// custom variables and functions that are defined in the other context
        /// </remarks>
        /// <param name="parent">The SifXPathContext to share custom functions and 
        /// variables with</param>
        /// <param name="sdo">The SIF Data Object or SIFElement to traverse</param>
        /// <returns>an instance of SifXPathContext</returns>
        public static SifXPathContext NewSIFContext( SifXPathContext parent,
                                                     SifElement sdo )
        {
            SifXPathContext context = new SifXPathContext( parent, sdo );

            return context;
        }

        /// <summary>
        /// Creates a new SifXPathContext instance to use for traversing the
        /// specified SIF Data Object
        /// </summary>
        /// <remarks>
        /// NOTE: The SIFDataObject.setSIFVersion(version) is automatically
        /// called and set to the target version.
        /// </remarks>
        /// <param name="parent">The SifXPathContext to share custom functions and 
        /// variables with</param>
        /// <param name="sdo">The SIF Data Object or SIFElement to traverse</param>
        /// <returns>an instance of SifXPathContext</returns>
        /// <param name="version">The SIFVersion to use when traversing this object using XPath.</param>
        public static SifXPathContext NewSIFContext( SifXPathContext parent,
                                                     SifElement sdo, SifVersion version )
        {
            sdo.SifVersion = version;
            return NewSIFContext( parent, sdo );
        }

        /// <summary>
        /// Creates a new SifXPathContext
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        private SifXPathContext( SifXPathContext parent, SifElement context )
        {
            if ( parent != null )
            {
                fContext = parent.fContext;
            }
            else
            {
                fContext = new SifXsltContext();
                fContext.AddFunctions( "adk", new ClassFunctions( typeof ( AdkFunctions ), null ) );
            }

            SifVersion version = context.SifVersion;
            if( version == null )
            {
                version = SifVersion.LATEST;
            }

            fContextElement = context;
            fContextPointer = new SifElementPointer( null, fContextElement, version );
            fDefaultNavigator = new SifXPathNavigator( fContext, fContextPointer );
        }


        /// <summary>
        /// Removes XPath syntax that was proprietary to the ADK in ADK 1.x versions and converts
        /// the expression to the syntax supported by JXPath
        /// </summary>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public static String ConvertLegacyXPath( String xPath )
        {
            StringBuilder sb = new StringBuilder( xPath );
            bool inPredicate = false;
            bool inString = false;
            int parens = 0;
            for ( int a = 0; a < sb.Length; a++ )
            {
                char chr = sb[a];
                switch ( chr )
                {
                    case '[':
                        inPredicate = true;
                        break;
                    case ']':
                        inPredicate = false;
                        break;
                    case '(':
                        parens++;
                        ;
                        break;
                    case ')':
                        parens--;
                        break;
                    case '\'':
                    case '"':
                        inString = !inString;
                        break;
                    case ',': // The ADK syntax assumes that a comma seperating predicates means " and "
                        if ( inPredicate && !inString && parens == 0 )
                        {
                            sb.Remove( a, 1 );
                            sb.Insert( a, " and " );
                            a += 4;
                        }
                        break;
                    case '$':
                        if ( sb[a + 1] == '(' )
                        {
                            //int closeParen = sb.indexOf(")", a);

                            int closeParen;
                            for ( closeParen = a; closeParen < sb.Length; closeParen++ )
                            {
                                if ( sb[closeParen] == ')' )
                                {
                                    break;
                                }
                            }


                            if ( sb[closeParen] == ')' )
                            {
                                sb.Remove( closeParen, 1 );
                                sb.Remove( a + 1, 1 );

                                if ( inString )
                                {
                                    // Remove the single quotes around this variable, if present
                                    if ( sb[a - 1] == '\'' )
                                    {
                                        sb.Remove( a - 1, 1 );
                                        inString = false;
                                    }
                                    if ( sb[closeParen - 2] == '\'' )
                                    {
                                        sb.Remove( closeParen - 2, 1 );
                                    }
                                }
                            }
                        }
                        break;
                    case '+': // The +] Syntax is ADK-specific and is used for outbound mappings
                        if ( inPredicate && !inString )
                        {
                            sb.Remove( a, 1 );
                            sb.Insert( a, " and adk:x()" );
                            a += 11;
                        }
                        break;
                }
            }

            return sb.ToString();
        }


        /**
	 * Allows for getting Elements from a SIF Element using the legacy ADK 1.x
	 * style XPath queries.
	 * 
	 * This method should only be used if the XPath syntax needs to be converted 
	 * from ADK 1.x syntax to true XPath. If the query is already in true XPath
	 * format, call {@link JXPathContext#getValue(java.lang.String)} 
	 * 
	 * @param xPath
	 * @return An Element from this object representing the path, or null
	 */

        public Element GetElementOrAttribute( String xPath )
        {
            String adkXPath = ConvertLegacyXPath( xPath );
            return GetValue( adkXPath ) as Element;
        }


        public void SetElementOrAttribute( String xPath, Object value )
        {
            String adkXPath = ConvertLegacyXPath( xPath );
            CreatePathAndSetValue( adkXPath, value );
        }


        /// <summary>
        /// Creates the specified path and returns a pointer
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public INodePointer CreatePath( String xpath )
        {
            INodePointer np = BuildADKPathWithPredicates( xpath, fContext );
            return np;
        }

        internal INodePointer CreatePath( SifXPathExpression sifXPathExpression )
        {
            return CreatePath( sifXPathExpression.Expression );
        }


        /// <summary>
        /// Creates the specified path and sets the value
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        /// <param name="value">The value to set</param>
        public void CreatePathAndSetValue( String xpath, object value )
        {
            IPointer np = BuildADKPathWithPredicates( xpath, fContext );
            np.SetValue( value );
        }

        /// <summary>
        /// Manually builds out a path to support the necessary mapping needs of the
        /// ADK. By default, the JXPath implementation does not allow
        /// context-dependend predicates (e.g. PhoneNumber[@Type='0096'] to be used
        /// in XPaths that create the path. This implementation manually steps
        /// through the XPath and builds it out. It's primary intent is to provide
        /// the behavior that was present in the ADK before JXPath was used for
        /// mapping
        /// </summary>
        /// <param name="expr">The Path expression to build out</param>
        /// <param name="context"></param>
        /// <returns></returns>
        private INodePointer BuildADKPathWithPredicates( String expr, XsltContext context )
        {
            // Use the set of expression steps to determine which parts of the
            // path already exist. Note that the order of evaluation used is optimized
            // for first-time creation of elements. In other words, the path chosen was
            // to evalaute the expression steps from the beginning rather than the end
            // because for outbound mappings, that order will generally be the most efficient
            AdkXPathStep[] steps = XPathParser.Parse( expr );
            int currentStep = 0;
            StringBuilder pathSoFar = new StringBuilder();
            INodePointer parent = fContextPointer;
            INodePointer current = null;
            for ( ; currentStep < steps.Length; currentStep++ )
            {
                current = FindChild( fDefaultNavigator, pathSoFar, steps[currentStep] );
                if ( current == null )
                {
                    break;
                }
                pathSoFar.Append( "/" );
                pathSoFar.Append( steps[currentStep].ToString() );
                parent = current;
            }
            if ( current != null )
            {
                // We traversed the entire path and came up with a result.
                // That means that the element we are trying to build the 
                // path to already exists. We will not create this path, so
                // return null;
                return null;
            }

            // We've traversed down to the level where we think we need to
            // add a child. However, there are cases where this is not the proper
            // location. For example, in SIF 1.5r1, the StudentAddressList element is 
            // repeatable and Address is not. It would not be proper to add a new Address
            // element under StudentAddressList. Instead, the algorithm needs to back
            // up the stack until it reaches the next repeatable element for the current
            // version of SIF
            // The following code is primarily in place for the StudentAddressList case, which is
            // why the isContextDependent() logic applies. Currently, there is no known other place
            // where this checking needs to occur.
            if ( currentStep > 0 && steps[currentStep].IsContextDependent() )
            {
                int step = currentStep;
                INodePointer stepParent = parent;
                while ( step > -1 )
                    // don't evaluate step 0 at the root of the object because this problem doesn't apply there
                {
                    if ( parent is SifElementPointer )
                    {
                        SifElementPointer sifParentPointer = (SifElementPointer) stepParent;
                        AdkNodeTest nt = steps[step].NodeTest;
                        if ( nt is AdkNodeNameTest )
                        {
                            SifElementPointer.AddChildDirective result =
                                sifParentPointer.GetAddChildDirective( ((AdkNodeNameTest) nt).NodeName );
                            if ( result != SifElementPointer.AddChildDirective.DONT_ADD_NOT_REPEATABLE )
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                    step--;
                    stepParent = stepParent.Parent;
                }
                if ( step > -1 && step != currentStep )
                {
                    currentStep = step;
                    parent = stepParent;
                }
            }

            // At this point, we have a parent element and the index of the current
            // step to evaluate
            //InitialContext context = new InitialContext( new RootContext( this, (NodePointer) getContextPointer()));
            for ( ; currentStep < steps.Length; currentStep++ )
            {
                AdkNodeTest nt = steps[currentStep].NodeTest;
                if ( nt is AdkNodeNameTest )
                {
                    current = parent.CreateChild( this, ((AdkNodeNameTest) nt).NodeName, 0 );
                    if ( current == null )
                    {
                        throw new ArgumentException( "Cannot evaluate expression step: " + steps[currentStep] );
                    }
                    foreach ( AdkExpression predicate in steps[currentStep].Predicates )
                    {
                        CreatePredicateValues( current, predicate, context );
                    }
                }
                else
                {
                    throw new ArgumentException( "Cannot evaluate expression step: " + steps[currentStep] );
                }

                parent = current;
            }
            // At the end, the 'parent' variable will contain the last element created by this function
            return parent;
        }

        private void CreatePredicateValues( INodePointer current, AdkExpression predicate, XsltContext evalContext )
        {
            if ( predicate is AdkEqualOperation )
            {
                AdkExpression[] components = ((AdkEqualOperation) predicate).Arguments;
                AdkLocPath lp = (AdkLocPath) components[0];
                AdkNodeNameTest attrName = (AdkNodeNameTest) lp.Steps[0].NodeTest;
                INodePointer attr = current.CreateAttribute( this, attrName.NodeName );
                Object value = components[1].ComputeValue( evalContext );
                attr.SetValue( value );
                return;
            }

            // This might be the 'adk:x()' function
            if ( predicate is AdkAndOperation )
            {
                foreach ( AdkExpression expr in ((AdkAndOperation) predicate).Arguments )
                {
                    if ( expr is AdkFunction && ((AdkFunction) expr).FunctionName.Equals( "adk:x" ) )
                    {
                        // This is the special marker function that tells the ADK to always
                        // create the parent repeatable element. Don't evaluate it.
                        continue;
                    }
                    else
                    {
                        CreatePredicateValues( current, expr, evalContext );
                    }
                }
                return;
            }

            // Unrecognized predicate
            throw new ArgumentException( "Cannot evaluate expression predicate: " + predicate );
        }


        /// <summary>
        ///  Evaluates the current step in the path. If the path represented by the step does not
        ///  exist, NULL is returned. Otherwise, the node found is returned unless the special adk:X() 
        ///  marker function is contained in the predicate expression, which signals that the specified
        /// repeatable element should always be created
        /// </summary>
        /// <param name="navigator"></param>
        /// <param name="parentPath"></param>
        /// <param name="currentStep"></param>
        /// <returns></returns>
        private INodePointer FindChild( XPathNavigator navigator, StringBuilder parentPath, AdkXPathStep currentStep )
        {
            String currentStepxPath = currentStep.ToString();
            if ( currentStep.IsContextDependent() )
            {
                // If the special 'adk:x()' function is present, that means to always 
                // create the element, therefore, return null as if it were not found
                if ( currentStepxPath.IndexOf( "adk:x" ) > -1 )
                {
                    return null;
                }
            }

            navigator.MoveToRoot();
            SifXPathNavigator sifNav =
                (SifXPathNavigator) navigator.SelectSingleNode( parentPath + "/" + currentStepxPath );
            if ( sifNav != null )
            {
                return sifNav.UnderlyingPointer;
            }
            return null;
        }

        /// <summary>
        /// Install a library of XPath Variables within a specific namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="variables"></param>
        public void AddVariables( String ns, IXPathVariableLibrary variables )
        {
            fContext.AddVariables( ns, variables );
        }

        /// <summary>
        /// Install a library of extension functions within a specific namespace
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="functions"></param>
        public void AddFunctions( String ns, IXPathFunctionLibrary functions )
        {
            fContext.AddFunctions( ns, functions );
        }

        ///<summary>
        ///Returns a new <see cref="T:System.Xml.XPath.XPathNavigator"></see> object. 
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Xml.XPath.XPathNavigator"></see> object.
        ///</returns>
        ///
        public XPathNavigator CreateNavigator()
        {
            return fDefaultNavigator.Clone();
        }


        /// <summary>
        /// Evaluates the xpath and returns the resulting object. Primitive types are wrapped into SimpleField
        /// objects. Complex Types are returned as a SIFElement
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public object GetValue( string s )
        {
            XPathExpression expression = fDefaultNavigator.Compile( s );
            return GetValue( expression );
        }

        /// <summary>
        /// Evaluates the xpath and returns the resulting object. Primitive types are wrapped into objects.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object GetValue( SifXPathExpression expression )
        {
            return GetValue( GetCompiledExpression( expression ) );
        }

        private XPathExpression GetCompiledExpression( SifXPathExpression expression )
        {
            XPathExpression returnValue = expression.CompiledExpression;
            if ( returnValue == null )
            {
                returnValue = fDefaultNavigator.Compile( expression.Expression );
                expression.CompiledExpression = returnValue;
            }
            return returnValue;
        }


        /// <summary>
        /// Evaluates the xpath and returns the resulting object. Primitive types are wrapped into objects.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private object GetValue( XPathExpression expression )
        {
            Object value = fDefaultNavigator.Evaluate( expression );
            XPathNodeIterator iterator = value as XPathNodeIterator;
            if ( iterator == null )
            {
                return value;
            }

            if ( iterator.MoveNext() )
            {
                return iterator.Current.TypedValue;
            }
            return null;
        }

        /// <summary>
        /// Evaluates the xpath and returns the resulting object. Primitive types are wrapped into objects.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public IPointer GetPointer( string s )
        {
            XPathExpression expression = fDefaultNavigator.Compile( s );
            Object value = fDefaultNavigator.Evaluate( expression );
            XPathNodeIterator iterator = value as XPathNodeIterator;
            if ( iterator == null )
            {
                return null;
            }
            if ( iterator.MoveNext() )
            {
                return ((SifXPathNavigator) iterator.Current).UnderlyingPointer;
            }
            return null;
        }


        public static SifXPathExpression Compile( string xPath )
        {
            String convertedXPath = ConvertLegacyXPath( xPath );
            return new SifXPathExpression( convertedXPath );
        }

        /// <summary>
        /// Selects a node set, using the specified XPath expression. 
        /// </summary>
        /// <param name="s">A String representing an XPath expression.</param>
        /// <returns>An XPathNodeIterator pointing to the selected node set. </returns>
        /// <seealso cref="System.Xml.XPath.XPathNavigator#Select(string)"/>
        public XPathNodeIterator Select( string s )
        {
            return fDefaultNavigator.Select( s );
        }
    }
}
