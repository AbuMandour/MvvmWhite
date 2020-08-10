using System;
using System.Globalization;
using Xamarin.Forms;
using NUnit.Framework;
using WhiteMvvmUnitTest.Mocks;


namespace WhiteMvvmUnitTest.BehaviorsTest
{
    public class EventToCommandBehaviorTests
    {
	    [SetUp]
	    public void Setup()
	    {
		    Xamarin.Forms.Mocks.MockForms.Init();
	    }
        [Test]
		public void InvalidEventNameShouldThrowArgumentExceptionTest()
		{
			var behavior = new MockEventToCommandBehavior
			{
				EventName = "OnItemTapped"
			};
			var listView = new ListView();

			Assert.Throws<ArgumentException>(() => listView.Behaviors.Add(behavior));
		}

		[Test]
		public void CommandExecutedWhenEventFiresTest()
		{
			bool executedCommand = false;
			var behavior = new MockEventToCommandBehavior
			{
				EventName = "ItemTapped",
				Command = new Command(() =>
				{
					executedCommand = true;
				})
			};
			var listView = new ListView();
			listView.Behaviors.Add(behavior);

			behavior.RaiseEvent(listView, null);

			Assert.True(executedCommand);
		}

		[Test]
		public void CommandCanExecuteTest()
		{
			var behavior = new MockEventToCommandBehavior
			{
				EventName = "ItemTapped",
				Command = new Command(() => Assert.True(false), () => false)
			};
			var listView = new ListView();
			listView.Behaviors.Add(behavior);

			behavior.RaiseEvent(listView, null);
		}

		[Test]
		public void CommandCanExecuteWithParameterShouldNotExecuteTest()
		{
			bool shouldExecute = false;
			var behavior = new MockEventToCommandBehavior
			{
				EventName = "ItemTapped",
				CommandParameter = shouldExecute,
				Command = new Command<string>(o => Assert.True(false), o => o.Equals(true))
			};
			var listView = new ListView();
			listView.Behaviors.Add(behavior);

			behavior.RaiseEvent(listView, null);
		}

		[Test]
		public void CommandWithConverterTest()
		{
			const string item = "ItemProperty";
			bool executedCommand = false;
			var behavior = new MockEventToCommandBehavior
			{
				EventName = "ItemTapped",
				EventArgsConverter = new ItemTappedEventArgsConverter(false),
				Command = new Command<string>(o =>
				{
					executedCommand = true;
					Assert.NotNull(o);
					Assert.AreEqual(item, o);
				})
			};
			var listView = new ListView();
			listView.Behaviors.Add(behavior);

			behavior.RaiseEvent(listView, new ItemTappedEventArgs(listView, item));

			Assert.True(executedCommand);
		}

		private class ItemTappedEventArgsConverter : IValueConverter
		{
			private readonly bool _returnParameter;

			public bool HasConverted { get; private set; }

			public ItemTappedEventArgsConverter(bool returnParameter)
			{
				_returnParameter = returnParameter;
			}

			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				HasConverted = true;
				return _returnParameter ? parameter : (value as ItemTappedEventArgs)?.Item;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}
    }
}