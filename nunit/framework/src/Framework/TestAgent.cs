using OpenADK.Library;
namespace Library.UnitTesting.Framework
{


   /// <summary>
   /// Summary description for TestAgent.
   /// </summary>
   public class TestAgent : Agent
   {
      private TestZoneFactory fZoneFactory;
      public TestAgent()
         : base("TestAgent")
      {
         fZoneFactory = new TestZoneFactory(this);
      }

      public override IZoneFactory ZoneFactory
      {
         get
         {
            return fZoneFactory;
        
         }
      }
   }
}