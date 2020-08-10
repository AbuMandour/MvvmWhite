using System;
using NUnit.Framework;
using WhiteMvvm.Behaviors;
using WhiteMvvmUnitTest.Mocks;
using Xamarin.Forms;


namespace WhiteMvvmUnitTest.BehaviorsTest
{
    public class EventToCommandBehaviorTests
    {
        public EventToCommandBehaviorTests()
            => Device.PlatformServices = new MockPlatformServices();

        [Test]
        public void ArgumentExceptionIfSpecifiedEventDoesNotExist()
        {
            var listView = new ListView();
            var behavior = new EventToCommandBehavior
            {
                EventName = "Wrong Event Name"
            };
            Assert.Throws<ArgumentException>(() => listView.Behaviors.Add(behavior));
        }

        [Test]
        public void NoExceptionIfSpecifiedEventExists()
        {
            var listView = new ListView();
            var behavior = new EventToCommandBehavior
            {
                EventName = nameof(ListView.ItemTapped)
            };
            listView.Behaviors.Add(behavior);
        }
    }
}