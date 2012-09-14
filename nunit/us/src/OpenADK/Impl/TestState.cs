using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using OpenADK.Library;

namespace Library.Nunit.US.Impl
{
    /**
    * @author administrator
    *
    */

    [Serializable]
    public class TestState : ISerializable
    {
        public string State
        {
            get { return fState; }
            set { fState = value; }
        }


        private String fState;

        /**
       *
       */
        private Boolean fCreateError;

        public TestState()
        {
        }

        public TestState(String state)
        {
            fState = state;
        }


        /// <summary>
        /// Overriden to test value equality. The underlying <see cref="SifEnum.Value"/> property is compared to 
        /// determine if it is equal.
        /// </summary>
        /// <param name="o">The SifEnum to compare against</param>
        /// <returns>True if the objects have the same value, otherwise False</returns>
        public override bool Equals(Object o)
        {
            // Test reference comparison first ( fastest )
            if (this == o)
            {
                return true;
            }
            if ((o != null) && (o.GetType().Equals(GetType())))
            {
                TestState comparedState = (TestState) o;
                if (State == null)
                {
                    return comparedState.State == null;
                }
                return State.Equals(comparedState.State);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if( State != null )
            {
                return State.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }


        public override String ToString()
        {
            return "TestState: " + State;
        }

        public void setCreateErrorOnRead(Boolean createError)
        {
            fCreateError = createError;
        }

        #region Serialization

        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fState", fState);
            info.AddValue("fCreateError", fCreateError);
        }


        // TODO: Andy E Serialization still does not work in the .Net ADK. We need to modify
        // adkgen so that the protected serialization constructor is added to each
        // subclass of Element

        // This is the serialization constructor.
        // Satisfies rule: ImplementSerializationConstructors.
        //[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected TestState(SerializationInfo info,
                            StreamingContext context)
        {
            fState = info.GetString("fState");
            fCreateError = info.GetBoolean("fCreateError");
            if (fCreateError)
            {
                throw new IOException("Errors Occurred during deserialization");
            }
        }

        #endregion
    } //end class
} //end namespace