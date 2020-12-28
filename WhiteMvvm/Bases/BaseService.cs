using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Services.Api;
using WhiteMvvm.Services.Cache.SqliteCache;
using WhiteMvvm.Services.DeviceUtilities;
using WhiteMvvm.Services.Localization;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Logging;

namespace WhiteMvvm.Bases
{
    public class BaseService
    {
        protected ILoggerService LoggerService;
        protected BaseService()
        {
            LoggerService = LocatorService.Instance.Resolve<ILoggerService>();            
        }

        protected static Language CurrentLanguage => LocalizationService.Current.CurrentLanguage;
    }
}
