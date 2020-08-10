using NUnit.Framework;
using WhiteMvvmUnitTest.Mocks;

namespace WhiteMvvmUnitTest.BasesTest
{
    public class BaseTransitionalTest
    {
        private MockTransitional _transitional;

        [SetUp]
        public void Setup()
        {
            _transitional = new MockTransitional();
        }

        [Test]
        public void TestMappingFromTransitionalToModel()
        {
          var resultModel =  _transitional.ToModel<MockTransitional, MockModel>(transitional => new MockModel()
          {
              Title = transitional?.ExternalTitle
          });
          Assert.NotNull(resultModel);
        }
    }
}