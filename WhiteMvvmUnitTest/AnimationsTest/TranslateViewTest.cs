using System;
using NUnit.Framework;
using WhiteMvvm.Animations;
using Xamarin.Forms.Xaml;

namespace WhiteMvvmUnitTest.AnimationsTest
{
    public class TranslateViewTest
    {
        [SetUp]
        public void Setup()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        [Test]
        [TestCase(10)]
        [TestCase(1000)]
        public void TranslateViewFormX(double xValue)
        {
            //arrange
            var label = new Xamarin.Forms.Label();
            var beforeLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var translateParameters = new TranslateParameters(xValue, 0, 100);
            //act 
            WhiteMvvm.Animations.Animation.SetTranslate(label,translateParameters);
            var afterLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var diff = afterLocation.X - beforeLocation.X;
            //assert
            Assert.IsTrue(Math.Abs(diff - xValue) < 0.001);
        }
        [Test]
        [TestCase(10)]
        [TestCase(1000)]
        public void TranslateViewFormY(double yValue)
        {
            //arrange
            var label = new Xamarin.Forms.Label();
            var beforeLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var translateParameters = new TranslateParameters(0, yValue, 100);
            //act 
            WhiteMvvm.Animations.Animation.SetTranslate(label,translateParameters);
            var afterLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var diff = afterLocation.Y - beforeLocation.Y;
            //assert
            Assert.IsTrue(Math.Abs(diff - yValue) < 0.001);
        }
        [Test]
        [TestCase(10,10)]
        [TestCase(1000,1000)]
        public void TranslateViewFormX_Y(double yValue,double xValue)
        {
            //arrange
            var label = new Xamarin.Forms.Label();
            var beforeLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var translateParameters = new TranslateParameters(xValue, yValue, 100);
            //act 
            WhiteMvvm.Animations.Animation.SetTranslate(label,translateParameters);
            var afterLocation = WhiteMvvm.Animations.Animation.GetTranslate(label);
            var diffX = afterLocation.X - beforeLocation.X;
            var diffY = afterLocation.Y - beforeLocation.Y;
            //assert
            Assert.IsTrue(Math.Abs(diffY - yValue) < 0.001 && Math.Abs(diffX - xValue) < 0.001);
        }
    }
}