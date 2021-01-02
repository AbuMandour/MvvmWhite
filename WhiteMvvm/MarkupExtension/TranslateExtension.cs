using System;
using System.Reflection;
using System.Resources;
using WhiteMvvm.Services.Localization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteMvvm.MarkupExtension
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        static readonly Assembly CallingAssembly = Assembly.GetCallingAssembly();

        static string ResourceId = $"{CallingAssembly.GetName().Name}.Resources.AppResource";
        
        static readonly Lazy<ResourceManager> resmgr = new Lazy<ResourceManager>(() => new ResourceManager(ResourceId, CallingAssembly));

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            
            var ci = LocalizationService.Current.CultureInfo;

            var translation = resmgr.Value.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
				translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}