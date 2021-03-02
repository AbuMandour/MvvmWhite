using System;
using System.Collections.Generic;
using System.Linq;
using WhiteMvvm.Bases;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Navigation;
using WhiteMvvm.Services.Resolve;
using WhiteMvvm.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Application = Xamarin.Forms.Application;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace WhiteMvvm
{
    public class WhiteApplication : Application
    {
        private static INavigationService? _navigationService;

        protected WhiteApplication()
        {

        }

        /// <summary>
        /// method to Initialize Navigation service in app class where we can write how we will begin app navigation 
        /// </summary>
        /// <param name="modal"></param>
        /// <returns></returns>
        protected static void SetHomePage(IModal modal)
        {
            _navigationService = LocatorService.Instance.Resolve<INavigationService>();
            _navigationService.SetMainModalAsync(modal).SafeFireAndForget(true);
        }

    }
}
