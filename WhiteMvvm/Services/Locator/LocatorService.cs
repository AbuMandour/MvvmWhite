using System;
using Unity;
using Unity.Lifetime;
using WhiteMvvm.Services.Api;
using WhiteMvvm.Services.Cache.SqliteCache;
using WhiteMvvm.Services.DeviceUtilities;
using WhiteMvvm.Services.Dialog;
using WhiteMvvm.Services.Logging;
using WhiteMvvm.Services.Navigation;
using WhiteMvvm.Services.Resolve;

namespace WhiteMvvm.Services.Locator
{
    public class LocatorService
    {
        private static readonly Lazy<LocatorService> Lazy = new Lazy<LocatorService>(() => new LocatorService());
        private readonly UnityContainer _container;
        private static bool _isRefreshing;

        public static LocatorService Instance
        {
            get
            {
                if (!_isRefreshing)
                    return Lazy.Value;
                _isRefreshing = false;
                return new LocatorService();
            }
        }
        public void RegisterBaseService()
        {
            _container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IConnectivity, ConnectivityService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IReflectionResolve, ReflectionResolve>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IApiService, ApiService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISqliteServiceAsync, SqliteServiceAsync>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IFileSystem, FileSystemService>(new ContainerControlledLifetimeManager());            
            _container.RegisterType<IMainThread, MainThreadService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILoggerService, LoggerService>(new ContainerControlledLifetimeManager());
        }
        private LocatorService()
        {
            _container = new UnityContainer().AddExtension(new Diagnostic()) as UnityContainer;
            RegisterBaseService();
        }
        private void MocksUpdateInternal(bool useMocks)
        {
            // Change injected dependencies
            if (useMocks)
            {
                _container.RegisterType<IConnectivity, Mocks.ConnectivityMockService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IDialogService, DialogMockService>(new ContainerControlledLifetimeManager());
            }
            else
            {
                _container.RegisterType<IConnectivity, ConnectivityService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            }
        }
        public T Resolve<T>() where T : class
        {
            return _container?.Resolve<T>();            
        }
        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }
        public void Register<TFrom, TTo>(ControlType lifetimeType) where TTo : TFrom
        {
            switch (lifetimeType)
            {
                case ControlType.SingleTone:
                    _container.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
                    break;
                case ControlType.Transient:
                    _container.RegisterType<TFrom, TTo>(new ContainerControlledTransientManager());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetimeType), lifetimeType, null);
            }
        }
        public void Register<T>(ControlType lifetimeType) where T : class
        {
            switch (lifetimeType)
            {
                case ControlType.SingleTone:
                    _container.RegisterType<T>(new ContainerControlledLifetimeManager());
                    break;
                case ControlType.Transient:
                    _container.RegisterType<T>(new ContainerControlledTransientManager());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetimeType), lifetimeType, null);
            }
        }
        public void RefreshLocator()
        {
            _isRefreshing = true;
        }
        public void MocksUpdate(bool useMocks)
        {
            MocksUpdateInternal(useMocks);
        }
    }
}
