using System.Net;

namespace WhiteMvvm.Services.Api
{
    public interface IProxyInfoProvider
    {
        WebProxy GetProxySettings();
    }
}