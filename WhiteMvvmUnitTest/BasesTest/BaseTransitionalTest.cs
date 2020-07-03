using NUnit.Framework;

namespace WhiteMvvmUnitTest.BasesTest
{
    public class BaseTransitionalTest
    {
        private TestableTransitional _transitional;

        [SetUp]
        public void Setup()
        {
            _transitional = new TestableTransitional();
        }

        [Test]
        public void TestMappingFromTransitionalToModel()
        {
          var resultModel =  _transitional.ToModel<TestableTransitional, TestableModel>(transitional => new TestableModel()
          {
              Title = transitional?.ExternalTitle
          });
          Assert.NotNull(resultModel);
        }
    }
}