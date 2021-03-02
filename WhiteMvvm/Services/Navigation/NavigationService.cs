using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WhiteMvvm.Bases;
using WhiteMvvm.Configuration;
using WhiteMvvm.Exceptions;
using WhiteMvvm.Services.Logging;
using WhiteMvvm.Services.Resolve;
using WhiteMvvm.Utilities;
using Xamarin.Forms;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace WhiteMvvm.Services.Navigation
{
    /// <summary>
    /// Navigation service to navigate between pages based on view model 
    /// </summary>
    public class NavigationService : INavigationService
    {
        private readonly IReflectionResolve _resolve;
        public async Task SetMainModalAsync(IModal modal)
        {
            var page = await GetPageFromModal(modal);
            Application.Current.MainPage = page;
        }
        private static INavigation? Navigation => Application.Current.MainPage?.Navigation;
        public NavigationService(IReflectionResolve resolve)
        {
            _resolve = resolve;
        }
        public bool SetMasterPresentation(bool isPresent)
        {
            MasterDetailPage masterDetailPage;
            if (Navigation.ModalStack != null && Navigation.ModalStack.Count > 1)
            {
                var lastModal = Navigation.ModalStack[Navigation.ModalStack.Count - 1];
                masterDetailPage = GetLastMasterDetailPage(lastModal);
            }
            else
            {
                masterDetailPage = GetLastMasterDetailPage(Application.Current.MainPage);
            }
            if (masterDetailPage != null)
            {
                masterDetailPage.IsPresented = isPresent;
                return true;
            }
            return false;
        }
        public async Task PopModelAsync(object parameter = null)
        {
            var previousViewModel = GetCurrentViewModel();
            previousViewModel.CleanUp();
            await Navigation.PopModalAsync(true);
            var currentViewModel = GetCurrentViewModel();
            await currentViewModel.OnNavigateFrom(previousViewModel, parameter);
        }
        public async Task PopFromNavigationModelAsync(object parameter = null)
        {
            var previousViewModel = GetCurrentViewModel();
            previousViewModel.CleanUp();
            var navigationPage = GetLastNavigationPage(null);
            if (navigationPage == null)
                throw new NavigationException("No navigation page to pop page");
            await navigationPage.PopAsync(true);
            var currentViewModel = GetCurrentViewModel();
            await currentViewModel.OnNavigateFrom(previousViewModel, parameter);
        }
        public void RemoveFromNavigationModalAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            var page = _resolve.CreatePage(typeof(TViewModel));
            Navigation.RemovePage(page);
        }
        public async Task PushToNavigationModalAsync<TViewModel>(object parameter = null) where TViewModel : BaseViewModel
        {
            try
            {
                var page = _resolve.CreatePage(typeof(TViewModel));
                if (page != null)
                {
                    var viewModel = _resolve.CreateViewModel(page.GetType());
                    page.BindingContext = viewModel;
                    var navigationPage = GetLastNavigationPage(null);
                    if (navigationPage != null)
                    {
                        viewModel.InternalInitialize(parameter).SafeFireAndForget();
                        await navigationPage.PushAsync(page, true);
                    }
                }
            }
            catch (NavigationException navigationException)
            {
                Console.WriteLine(navigationException.Message);
                throw new NavigationException("Error while push page", navigationException);
            }
        }
        public async Task<bool> ChangeDetailAsync(IModal modal)
        {
            if (Application.Current.MainPage == null)
                return false;

            var masterDetailPage = GetLastMasterDetailPage(null);
            if (masterDetailPage == null)
                return false;
            //set detail 
            switch (modal)
            {
                case NavigationModal navigationModal:
                    var navigationPage = await SetNavigationPageAsync(navigationModal);
                    masterDetailPage.Detail = navigationPage;
                    return true;
                case BasicModal basicModal:
                    var basicPage = SetBasicPage(basicModal);
                    masterDetailPage.Detail = basicPage;
                    return true;
                case TabbedModal tabbedModal:
                    var tabbedPage = await SetTabbedPageToMasterDetailAsync(tabbedModal);
                    masterDetailPage.Detail = tabbedPage;
                    return true;
                default:
                    return false;
            }
        }
        public async Task PushModalAsync(IModal modal)
        {
            var page = await GetPageFromModal(modal);
            await PushModalGlobal(page);
        }
        public void ChangeCurrentTabbedModal<TBaseViewModel>() where TBaseViewModel : BaseViewModel
        {
            var currentTabbedPage = GetLastTabbedPage(null);
            var page = currentTabbedPage.Children.Select(GetLastContentPage).FirstOrDefault(basePage => basePage.BindingContext is TBaseViewModel);
            currentTabbedPage.CurrentPage = page ?? throw new NavigationException("page you requested not found");
        }
        public TabbedPage CurrentTabbedPage => GetLastTabbedPage(null);
        private static BaseViewModel GetCurrentViewModel()
        {
            var currentPage = GetCurrentPage();
            var currentViewModel = currentPage.BindingContext as BaseViewModel;
            return currentViewModel;
        }
        private static BaseContentPage GetCurrentPage()
        {
            if (Navigation.ModalStack.Count > 0)
            {
                var lastPage = Navigation.ModalStack[Navigation.ModalStack.Count - 1];
                return GetLastContentPage(lastPage);
            }
            var mainPage = Application.Current.MainPage;
            return GetLastContentPage(mainPage);
        }
        private static BaseContentPage GetLastContentPage(Page page)
        {
            var pageType = page.GetType();
            while (pageType.BaseType != typeof(BaseContentPage))
            {
                switch (page)
                {
                    case NavigationPage navigationPage:
                        return GetLastContentPage(navigationPage.CurrentPage);
                    case MasterDetailPage masterDetailPage:
                        return GetLastContentPage(masterDetailPage.Detail);
                    case TabbedPage tabbedPage:
                        return GetLastContentPage(tabbedPage.CurrentPage);
                    default:
                        return page as BaseContentPage;
                }
            }
            return page as BaseContentPage;
        }
        private static NavigationPage GetLastNavigationPage(Page page)
        {
            if (page == null)
            {
                var lastModal = Navigation.ModalStack.Count > 0 ? Navigation.ModalStack[Navigation.ModalStack.Count - 1] : Application.Current.MainPage;
                page = lastModal;
            }
            var pageType = page.GetType();
            while (pageType != typeof(NavigationPage))
            {
                switch (page)
                {
                    case MasterDetailPage masterDetailPage:
                        return GetLastNavigationPage(masterDetailPage.Detail);
                    case TabbedPage tabbedPage:
                        return GetLastNavigationPage(tabbedPage.CurrentPage);
                    default:
                        throw new NavigationException("No Navigation page to push page");
                }
            }
            return page as NavigationPage;
        }
        private static MasterDetailPage? GetLastMasterDetailPage(Page root)
        {
            if (root == null)
            {
                var lastModal = Navigation.ModalStack.Count > 0 ? Navigation.ModalStack[Navigation.ModalStack.Count - 1] : Application.Current.MainPage;
                root = lastModal;
            }
            var pageType = root.GetType();
            while (pageType != typeof(MasterDetailPage))
            {
                switch (root)
                {
                    case NavigationPage navigationPage:
                        return GetLastMasterDetailPage(navigationPage.CurrentPage);
                    case TabbedPage tabbedPage:
                        return GetLastMasterDetailPage(tabbedPage.CurrentPage);
                    default:
                        return root as MasterDetailPage;
                }
            }
            return root as MasterDetailPage;
        }
        private static TabbedPage? GetLastTabbedPage(Page root)
        {
            if (root == null)
            {
                var lastModal = Navigation.ModalStack.Count > 0 ? Navigation.ModalStack[Navigation.ModalStack.Count - 1] : Application.Current.MainPage;
                root = lastModal;
            }
            var pageType = root.GetType();
            while (pageType != typeof(TabbedPage))
            {
                switch (root)
                {
                    case NavigationPage navigationPage:
                        return GetLastTabbedPage(navigationPage.CurrentPage);
                    case MasterDetailPage masterDetailPage:
                        return GetLastTabbedPage(masterDetailPage.Detail);
                    default:
                        return root as TabbedPage;
                }
            }
            return root as TabbedPage;
        }
        private static async Task PushModalGlobal(Page page)
        {
            if (Navigation != null)
            {
                await Navigation.PushModalAsync(page);
            }

            else
            {
                Application.Current.MainPage = page;
            }
        }
        private Task<Page> GetPageFromModal(IModal modal)
        {
            switch (modal)
            {
                case BasicModal basicModal:
                    return Task.FromResult(SetBasicPage(basicModal));
                case NavigationModal navigationModal:
                    return SetNavigationPageAsync(navigationModal);
                case TabbedModal tabbedModal:
                    return SetTabbedModal(tabbedModal);
                case MasterDetailModal masterDetailModal:
                    return SetMasterDetailModal(masterDetailModal);
                default:
                    throw new NavigationException("Navigation service doesn't support given modal");
            }
        }
        private Page SetBasicPage(BasicModal basicModal)
        {
            var basicViewModel = basicModal.ViewModel;
            var basicPage = _resolve.CreatePage(basicViewModel.GetType());
            basicPage.BindingContext = basicViewModel;
            basicViewModel.InternalInitialize(basicModal.NavigationParameter).SafeFireAndForget();
            return basicPage;
        }
        private async Task<Page> SetNavigationPageAsync(NavigationModal navigationModal)
        {
            Page rootPage;
            switch (navigationModal.RootModal)
            {
                case BasicModal basicModal:
                    rootPage = SetBasicPage(basicModal);
                    break;
                case TabbedModal tabbedModal:
                    rootPage = await SetTabbedPageToMasterDetailAsync(tabbedModal);
                    break;
                case MasterDetailModal masterDetailModal:
                    rootPage = await SetMasterDetailPageForTabbedPage(masterDetailModal);
                    break;
                default:
                    throw new NavigationException("Tabbed page doesn't support given modal type at this level");
            }
            await navigationModal.NavigationPage.PushAsync(rootPage);
            return navigationModal.NavigationPage;
        }
        private async Task<Page> SetMasterDetailModal(MasterDetailModal masterDetailModal)
        {
            //check master detail page
            if (masterDetailModal.MasterDetailPage == null)
                masterDetailModal.MasterDetailPage = new MasterDetailPage();
            //set detail 
            switch (masterDetailModal.DetailModal)
            {
                case NavigationModal navigationModal:
                    var navigationPage = await SetNavigationPageAsync(navigationModal);
                    masterDetailModal.MasterDetailPage.Detail = navigationPage;
                    break;
                case BasicModal basicModal:
                    var basicPage = SetBasicPage(basicModal);
                    masterDetailModal.MasterDetailPage.Detail = basicPage;
                    break;
                case TabbedModal tabbedModal:
                    var tabbedPage = await SetTabbedPageToMasterDetailAsync(tabbedModal);
                    masterDetailModal.MasterDetailPage.Detail = tabbedPage;
                    break;
                default:
                    throw new NavigationException("Master Detail page doesn't support given modal type at this level");
            }
            //set master
            if (masterDetailModal.MasterModal is BasicModal masterModal)
            {
                var masterPage = SetBasicPage(masterModal);
                // check on title in master page
                if (string.IsNullOrEmpty(masterPage.Title))
                    masterPage.Title = "master page";
                masterDetailModal.MasterDetailPage.Master = masterPage;
                // set modal in navigation stack
                return masterDetailModal.MasterDetailPage;
            }
            throw new NavigationException("Cannot set master page given modal type");
        }
        private async Task<Page> SetTabbedModal(TabbedModal tabbedModal)
        {
            //check modals count
            if (tabbedModal.Modals.Count <= 1)
                throw new NavigationException("tabbed page must be more than one page");
            //check tabbed page
            if (tabbedModal.TabbedPage == null)
                tabbedModal.TabbedPage = new TabbedPage();
            //add modals to tabbed page
            foreach (var modal in tabbedModal.Modals)
            {
                switch (modal)
                {
                    case BasicModal basicModal:
                        var basicPage = SetBasicPage(basicModal);
                        tabbedModal.TabbedPage.Children.Add(basicPage);
                        break;
                    case NavigationModal navigationModal:
                        var navigationPage = await SetNavigationPageAsync(navigationModal);
                        tabbedModal.TabbedPage.Children.Add(navigationPage);
                        break;
                    case MasterDetailModal masterDetailModal:
                        var masterDetailPage = await SetMasterDetailPageForTabbedPage(masterDetailModal);
                        tabbedModal.TabbedPage.Children.Add(masterDetailPage);
                        break;
                    default:
                        throw new NavigationException("Tabbed page doesn't support given modal type at this level");
                }
            }
            // set modal in navigation stack
            return tabbedModal.TabbedPage;
        }
        private async Task<Page> SetTabbedPageToMasterDetailAsync(TabbedModal tabbedModal)
        {
            //check modals count
            if (tabbedModal.Modals.Count <= 1)
                throw new NavigationException("tabbed page must be more than one page");
            //check tabbed page
            if (tabbedModal.TabbedPage == null)
                tabbedModal.TabbedPage = new TabbedPage();
            //add modals to tabbed page
            foreach (var modal in tabbedModal.Modals)
            {
                switch (modal)
                {
                    case BasicModal basicModal:
                        var basicPage = SetBasicPage(basicModal);
                        tabbedModal.TabbedPage.Children.Add(basicPage);
                        break;
                    case NavigationModal navigationModal:
                        var navigationPage = await SetNavigationPageAsync(navigationModal);
                        tabbedModal.TabbedPage.Children.Add(navigationPage);
                        break;
                    default:
                        throw new NavigationException("Tabbed page doesn't support given modal type at this level");
                }
            }
            return tabbedModal.TabbedPage;
        }
        private async Task<Page> SetMasterDetailPageForTabbedPage(MasterDetailModal masterDetailModal)
        {
            //check master detail page
            if (masterDetailModal.MasterDetailPage == null)
                masterDetailModal.MasterDetailPage = new MasterDetailPage();
            //set detail 
            switch (masterDetailModal.DetailModal)
            {
                case NavigationModal navigationModal:
                    var internalNavigationPage = await SetNavigationPageAsync(navigationModal);
                    masterDetailModal.MasterDetailPage.Detail = internalNavigationPage;
                    break;
                case BasicModal basicModal:
                    var internalBasicPage = SetBasicPage(basicModal);
                    masterDetailModal.MasterDetailPage.Detail = internalBasicPage;
                    break;
                default:
                    throw new NavigationException("Master Detail page doesn't support given modal type at this level");
            }
            //set master
            //ToDo Check for not basic modal
            var masterModel = (BasicModal)masterDetailModal.MasterModal;
            var masterPage = SetBasicPage(masterModel);
            // check on title in master page
            if (string.IsNullOrEmpty(masterPage.Title))
                masterPage.Title = "master page";
            masterDetailModal.MasterDetailPage.Master = masterPage;
            return masterDetailModal.MasterDetailPage;
        }

    }
}
