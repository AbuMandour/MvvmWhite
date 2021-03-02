using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public interface INavigationService
    {
        Task SetMainModalAsync(IModal modal);
        Task PushModalAsync(IModal modal);
        Task PopModelAsync(object parameter = null);
        Task PopFromNavigationModelAsync(object parameter = null);
        void RemoveFromNavigationModalAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task PushToNavigationModalAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;        
        Task<bool> ChangeDetailAsync(IModal modal);
        bool SetMasterPresentation(bool isPresent);
        void ChangeCurrentTabbedModal<TBaseViewModel>() where TBaseViewModel : BaseViewModel;
        TabbedPage CurrentTabbedPage { get; }
    }
}
