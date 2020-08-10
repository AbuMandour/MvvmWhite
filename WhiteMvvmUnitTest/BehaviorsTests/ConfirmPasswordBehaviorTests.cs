using System;
using NUnit.Framework;
using WhiteMvvm.Behaviors;
using WhiteMvvm.Behaviors.Base;
using WhiteMvvmUnitTest.Mocks;
using Xamarin.Forms;

namespace WhiteMvvmUnitTest.BehaviorsTest
{
public class ConfirmPasswordBehaviorTests
    {
        public ConfirmPasswordBehaviorTests()
             => Device.PlatformServices = new MockPlatformServices();
        
        [Test]
        public void IsValidTrueWhenBothIsNull_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;
            //act
            //passwordEntry.Text = "123456";
            confirmPasswordBehavior.OriginalPassword = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            //confirmPasswordEntry.Text = "123456";
            //assert
            Assert.True(confirmPasswordBehavior.IsValid);
        }
        
        [Test]
        public void IsValidFalseWhenOneIsNull_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnAttaching;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.OriginalPassword = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = null;
            //assert
            Assert.False(confirmPasswordBehavior.IsValid);
        }

        [Test]
        public void IsValidTrueWhenEnterSameText_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.OriginalPassword = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "123456";
            //assert
            Assert.True(confirmPasswordBehavior.IsValid);
        }
        
        [Test]
        public void IsValidFalseWhenEnterDifferentText_Test()
        {
            //arrange
            var passwordEntry = new Entry();
            var confirmPasswordEntry = new Entry();
            var confirmPasswordBehavior = new ConfirmPasswordBehavior();
            confirmPasswordBehavior.Flags = ValidationFlags.ValidateOnValueChanging;
            //act
            passwordEntry.Text = "123456";
            confirmPasswordBehavior.OriginalPassword = passwordEntry.Text;
            confirmPasswordEntry.Behaviors.Add(confirmPasswordBehavior);
            confirmPasswordEntry.Text = "1234567";
            //assert
            Assert.False(confirmPasswordBehavior.IsValid);
        }
    }
}