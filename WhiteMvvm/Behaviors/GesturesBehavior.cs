using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WhiteMvvm.Behaviors
{
    public static class GesturesBehavior
    {
        public static readonly BindableProperty TapCommandProperty =
            BindableProperty.CreateAttached("TapCommand", typeof(ICommand), typeof(GesturesBehavior), null, propertyChanged: OnTapCommandChanged);

        public static ICommand GetTapCommand(BindableObject view)
            => (ICommand)view.GetValue(TapCommandProperty);

        public static void SetTapCommand(BindableObject view, ICommand value)
            => view.SetValue(TapCommandProperty, value);

        static void OnTapCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is View view && newValue is ICommand tapCommand)
                view.GestureRecognizers.Add(new TapGestureRecognizer { Command = tapCommand });
        }
    }
}
