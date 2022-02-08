using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using WhiteMvvm.Services.Locator;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace WhiteMvvm.Services.Localization
{
    public class LocalizationService : ObservableObject
    {
        private static readonly Lazy<LocalizationService> Lazy = new Lazy<LocalizationService>(() => new LocalizationService());
        public static LocalizationService Current => Lazy.Value;
        private LocalizationService()
        {
        }
        private FlowDirection _appFlowDirection;
        private CultureInfo _cultureInfo;
        public CultureInfo CultureInfo
        {
            get
            {
                if (_cultureInfo == null)
                    return CultureInfo.CurrentCulture;
                return _cultureInfo;
            }
            set => SetProperty(ref _cultureInfo, value);
            
        }

        public FlowDirection AppFlowDirection
        {
            get => _appFlowDirection;
            set => SetProperty(ref _appFlowDirection, value);
           
        }
        public Language CurrentLanguage
        {
            get
            {
                CultureInfo ci = Current.CultureInfo;
                if (ci.ToString().Contains("ar") || ci.ToString().Contains("Ar"))
                {
                    return Language.Arabic;
                }
                if (ci.ToString().Contains("en") || ci.ToString().Contains("En"))
                {
                    return Language.English;
                }
                return Language.English;
            }
        }
        public async Task SetCurrentLanguage(Language language,Action<CultureInfo> afterCultureSet, bool isRefreshing = true)
        {
            string languageName = "";
            switch (language)
            {
                case Language.English:
                {
                    languageName = "en";
                    CultureInfo englishCulture = new CultureInfo(languageName);
                    Current.CultureInfo = englishCulture;
                    Current.AppFlowDirection = FlowDirection.LeftToRight;
                    afterCultureSet.Invoke(englishCulture);
                    if (Application.Current.Properties.ContainsKey("CurrentLanguage"))
                    {
                        Application.Current.Properties.Remove("CurrentLanguage");
                    }
                    Application.Current.Properties.Add("CurrentLanguage", "English");
                    await Application.Current.SavePropertiesAsync();
                    break;
                }
                case Language.Arabic:
                {
                    languageName = "ar";
                    CultureInfo arabicCulture = new CultureInfo(languageName);
                    Current.CultureInfo = arabicCulture;
                    Current.AppFlowDirection = FlowDirection.RightToLeft;
                    afterCultureSet.Invoke(arabicCulture);
                    if (Application.Current.Properties.ContainsKey("CurrentLanguage"))
                    {
                        Application.Current.Properties.Remove("CurrentLanguage");
                    }
                    Application.Current.Properties.Add("CurrentLanguage", "Arabic");
                    await Application.Current.SavePropertiesAsync();
                    break;
                }
            }
            if (isRefreshing)
            {
                LocatorService.Instance.RefreshLocator();
            }
            
        }
    }
}