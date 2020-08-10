using System;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Resolve
{
    public interface IReflectionResolve
    {
        BaseViewModel CreateViewModel(Type pageType);
        Page CreatePage(Type viewModelType);
    }
}
