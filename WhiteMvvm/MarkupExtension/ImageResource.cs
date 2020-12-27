using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteMvvm.MarkupExtension
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var assembly = Assembly.GetCallingAssembly();
            var imageSource = ImageSource.FromResource(Source, assembly);
            return imageSource;
        }
    }
}