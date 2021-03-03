using System;
using System.Threading.Tasks;
using WhiteMvvm.Configuration;
using WhiteMvvm.Services.DeviceUtilities;
using WhiteMvvm.Services.Dialog;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Logging;
using WhiteMvvm.Services.Navigation;
using WhiteMvvm.Utilities;
using Xamarin.Forms;

namespace WhiteMvvm.Bases
{
    public class BaseViewModel : NotifiedObject
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IConnectivity ConnectivityService;
        protected readonly IMainThread MainThreadService;
        protected readonly ILoggerService LoggerService;

        private volatile bool _isInitialize;
        private bool _isBusy = true;
        private volatile bool _isOnAppeared;
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }
        private object NavigationData { get; set; }
        protected BaseViewModel()
        {
            DialogService = LocatorService.Instance.Resolve<IDialogService>();
            NavigationService = LocatorService.Instance.Resolve<INavigationService>();
            ConnectivityService = LocatorService.Instance.Resolve<IConnectivity>();
            MainThreadService = LocatorService.Instance.Resolve<IMainThread>();
            LoggerService = LocatorService.Instance.Resolve<ILoggerService>();            
        }
        protected internal virtual void CleanUp()
        {
            _isOnAppeared = false;
            _isInitialize = false;
        }
        protected internal virtual Task OnNavigateFrom(BaseViewModel? page, object? parameter)
        {
            return Task.CompletedTask;
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (ConfigurationManager.Current.UseBasicIndicator)
                    {
                        if (value)
                        {
                            DialogService.ShowLoading(ConfigurationManager.Current.IndicatorMaskType);
                        }
                        else
                        {
                            DialogService.HideLoading();
                        }
                    }
                    _isBusy = value;
                    OnPropertyChanged();
                });
            }
        }
        protected internal virtual Task OnPopupAppearing()
        {
            return Task.CompletedTask;
        }
        protected internal virtual Task OnPopupDisappearing()
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnAppearing(object view)
        {
            return Task.CompletedTask;
        }
        protected internal virtual Task OnDisappearing(object view)
        {
            return Task.CompletedTask;
        }
        protected virtual Task InitializeAsync(object navigationData)
        {
            NavigationData = navigationData;
            return Task.CompletedTask;
        }
        protected virtual Task InitializeVolatileAsync(object navigationData)
        {
            NavigationData = navigationData;
            return Task.CompletedTask;
        }
        internal async Task InternalInitialize(object? navigationData)
        {
            await InitializeVolatileAsync(navigationData);
                
            if (_isInitialize)
                return;
            _isInitialize = true;
            await InitializeAsync(navigationData);
        }
        protected virtual Task OnAppeared()
        {
            return Task.CompletedTask;
        }
        internal Task InternalOnAppear(object view)
        {
            OnAppearing(view).SafeFireAndForget();
            if (_isOnAppeared)
                return Task.CompletedTask;
            _isOnAppeared = true;
            OnAppeared().SafeFireAndForget();
            return Task.CompletedTask;
        }
        protected internal virtual bool HandleBackButton()
        {
            return false;
        }
    }
}
