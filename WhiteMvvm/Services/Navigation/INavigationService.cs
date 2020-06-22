using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiteMvvm.Bases;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Navigation
{
    public interface INavigationService
    {        
        bool SetMasterPresentation(bool isPresent);
        Task PopModelAsync(object parameter = null);
        Task PopAsync(object parameter = null);
        void RemovePageFromNavigationModalAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel;        
        Task<bool> ChangeDetail(IModal modal);
        Task PushModal(IModal modal);
        Task SetMainModal(IModal modal);
        void ChangeCurrentTabbedModal<TBaseViewModel>() where TBaseViewModel : BaseViewModel;
        BaseTabbedPage CurrentTabbedPage { get;}
    }
}
