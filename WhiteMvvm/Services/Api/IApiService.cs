using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhiteMvvm.Bases;

namespace WhiteMvvm.Services.Api
{
    public interface IApiService
    {
        Task<TBaseTransitional> Get<TBaseTransitional>(string uri, Dictionary<string, string> headers = null)
            where TBaseTransitional : BaseTransitional;
        Task<List<TBaseTransitional>> GetList<TBaseTransitional>(string uri, Dictionary<string, string> headers = null) where TBaseTransitional : BaseTransitional, new ();
        Task<TResponse> Post<TResponse, TRequest>(TRequest entity,  string contentType, string uri ,Dictionary<string, string> headers= null) where TRequest : BaseTransitional where TResponse : class;
         Task<string> GetRedirect(string uri, Dictionary<string, string> headers = null);
    }
}
