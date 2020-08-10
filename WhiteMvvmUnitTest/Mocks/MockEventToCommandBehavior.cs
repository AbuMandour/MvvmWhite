using System.Xml.Schema;
using WhiteMvvm.Behaviors;

namespace WhiteMvvmUnitTest.Mocks
{
    public class MockEventToCommandBehavior: EventToCommandBehavior
    {
        public void RaiseEvent(params object[] args)
        {
            _handler.DynamicInvoke(args);
        }
    }
}