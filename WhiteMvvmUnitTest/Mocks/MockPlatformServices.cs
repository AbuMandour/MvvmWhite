using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace WhiteMvvmUnitTest.Mocks
{
    public class MockPlatformServices : IPlatformServices
    {
        public string GetHash(string input) => string.Empty;
 
        public string GetMD5Hash(string input)
            => string.Empty;

        public double GetNamedSize(NamedSize size, Type targetElement, bool useOldSizes)
            => 0;

        public Color GetNamedColor(string name)
        {
            return Color.FromHex(name);
        }

        public void OpenUriAction(Uri uri)
        {
        }

        public bool IsInvokeRequired
            => false;

        public OSAppTheme RequestedTheme { get; }

        public string RuntimePlatform { get; set; }

        public void BeginInvokeOnMainThread(Action action)
            => action();

        public Ticker CreateTicker()
            => new MockTicker();

        public void StartTimer(TimeSpan interval, Func<bool> callback)
        {
        }

        public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
            => Task.FromResult<Stream>(new MemoryStream());

        public Assembly[] GetAssemblies()
            => new Assembly[0];

        public IIsolatedStorageFile GetUserStoreForApplication()
            => null;

        Assembly[] IPlatformServices.GetAssemblies()
            => new Assembly[0];

        public void QuitApplication()
        {
        }

        public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint)
            => new SizeRequest();
    }
}