using System;
using Xamarin.Forms;

namespace WhiteMvvm.Behaviors
{
    public class ConfirmPasswordBehavior : Behavior<Entry>
    {
        private static readonly BindablePropertyKey IsSamePasswordPropertyKey =
            BindableProperty.CreateReadOnly("IsSamePassword",
                typeof(bool), typeof(ConfirmPasswordBehavior), false);
        public static readonly BindableProperty IsSamePasswordProperty = IsSamePasswordPropertyKey.BindableProperty;

        public static readonly BindableProperty CompareToTextProperty =
            BindableProperty.Create("CompareToText", typeof(string),
                typeof(ConfirmPasswordBehavior), string.Empty);

        public string CompareToText
        {
            get => (string)base.GetValue(CompareToTextProperty);
            set => base.SetValue(CompareToTextProperty, value);
        }
        public bool IsSamePassword
        {
            get => (bool)base.GetValue(IsSamePasswordProperty);
            private set => base.SetValue(IsSamePasswordPropertyKey, value);
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            string password = CompareToText;
            string confirmPassword = e.NewTextValue;
            if (string.IsNullOrEmpty(password))
            {
                IsSamePassword = false;
                return;
            }
            IsSamePassword = password.Equals(confirmPassword);
        }
    }
}
