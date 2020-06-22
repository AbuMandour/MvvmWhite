using System;
using System.Collections.Generic;
using System.Text;
using WhiteMvvm.Services.Api;
using WhiteMvvm.Services.Cache.SqliteCache;
using WhiteMvvm.Services.DeviceUtilities;
using WhiteMvvm.Services.Locator;
using WhiteMvvm.Services.Logging;

namespace WhiteMvvm.Bases
{
    public class BaseRepoService : BaseService
    {
        protected IConnectivity Connectivity;
        protected IApiService ApiService;
        protected ISqliteServiceAsync SqliteService;
        public BaseRepoService()
        {
            ApiService = LocatorService.Instance.Resolve<IApiService>();
            Connectivity = LocatorService.Instance.Resolve<IConnectivity>();
            SqliteService = LocatorService.Instance.Resolve<ISqliteServiceAsync>();
        }
    }
}
