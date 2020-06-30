using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Resolve;
using WhiteMvvm.Utilities;
using Xamarin.Forms;

namespace WhiteMvvm.Bases
{
    public class BaseContentView : ContentView
    {
        protected internal virtual void OnDisappearing()
        {
            ViewModel?.OnDisappearing(this).SafeFireAndForget();
        }
        protected internal virtual void OnAppearing()
        {
            ViewModel?.InternalOnAppear(this).SafeFireAndForget();
        }
        private BaseViewModel? ViewModel => BindingContext as BaseViewModel;
    }
}
