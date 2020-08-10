using System;
using NUnit.Framework;
using WhiteMvvm.Behaviors;
using Xamarin.Forms;

namespace WhiteMvvmUnitTest.BehaviorsTest
{
    public class ConfirmPasswordBehaviorTest
    {
        [SetUp]
        public void Setup()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }
        [Test]
        public void IsSamePasswordTrueWhenEnterSameTextTest()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.CompareToText = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "123456";
            //assert
            Assert.IsTrue(confirmPasswordBehavior.IsSamePassword);
        }
        [Test]
        public void IsSamePasswordFalseWhenEnterDifferentTextTest()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.CompareToText = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "1234567";
            //assert
            Assert.IsFalse(confirmPasswordBehavior.IsSamePassword);
        }
    }
}