using OpenADK.Library;
using OpenADK.Library.Tools.Mapping;

namespace Library.Nunit.US.Tools.Mapping
{
    internal class TestValueBuilder : DefaultValueBuilder
    {
        private bool fWasCalled;

        public TestValueBuilder( IFieldAdaptor data ) : base( data )
        {
        }

        public bool WasCalled
        {
            get { return fWasCalled; }
            set { fWasCalled = value; }
        }
    }
}