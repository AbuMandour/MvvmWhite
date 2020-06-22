using WhiteMvvm.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;


namespace WhiteMvvm.Bases
{
    public class BaseContentPage : ContentPage
    {
        protected BaseContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
        private BaseViewModel ViewModel => BindingContext as BaseViewModel;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            AppearingIterateChildren(this);
            ViewModel?.InternalOnAppear().SafeFireAndForget();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            DisappearingIterateChildren(this);
            ViewModel?.OnDisappearing().SafeFireAndForget();
        }
        protected override bool OnBackButtonPressed()
        {
            var result = ViewModel?.HandleBackButton();
            return result ?? false;
        }
        private static void AppearingIterateChildren(IElementController content)
        {
            var children = content?.LogicalChildren;
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (child.GetType().IsSubclassOf(typeof(BaseContentView)))
                    {
                        var baseContentView = child as BaseContentView;
                        baseContentView?.OnAppearing();
                    }
                    else
                    {
                        AppearingIterateChildren(child);
                    }
                }
            }
        }
        private static void DisappearingIterateChildren(IElementController content)
        {
            var children = content?.LogicalChildren;
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (child.GetType().IsSubclassOf(typeof(BaseContentView)))
                    {
                        var baseContentView = child as BaseContentView;
                        baseContentView?.OnDisappearing();
                    }
                    else
                    {
                        DisappearingIterateChildren(child);
                    }
                }
            }
        }
    }
}
