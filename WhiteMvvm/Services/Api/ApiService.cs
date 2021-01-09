using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WhiteMvvm.Bases;
using Xamarin.Forms;


namespace WhiteMvvm.Services.Api
{
    public class ApiService : IApiService
    {
        private  HttpClient _httpClient = new HttpClient();
        private readonly JsonSerializer _serializer = new JsonSerializer();
        public async Task<TBaseTransitional> Get<TBaseTransitional>(string uri, Dictionary<string, string> headers = null) where TBaseTransitional : BaseTransitional
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, errors) => true;
            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var json = new JsonTextReader(reader))
            {
                return _serializer.Deserialize<TBaseTransitional>(json);
            }
        }
        public async Task<List<TBaseTransitional>> GetList<TBaseTransitional>(string uri, Dictionary<string, string> headers = null) where TBaseTransitional : BaseTransitional, new()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            using (var json = new JsonTextReader(reader))
            {
                var list = _serializer.Deserialize<List<TBaseTransitional>>(json);
                return list;
            }
        }
        public async Task<TResponse> Post<TResponse, TRequest>(TRequest entity, string contentType, string uri, Dictionary<string, string> headers = null) where TRequest : BaseTransitional
            where
            TResponse : class
        {
            if (headers != null)
            {
                _httpClient.DefaultRequestHeaders.Clear();
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            StringContent dateContent = null;
            if (entity != null)
            {
                var json = JsonConvert.SerializeObject(entity);
                dateContent = new StringContent(json, Encoding.UTF8, contentType);
            }
            var response = await _httpClient.PostAsync(uri, dateContent);
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeObject<TResponse>(jsonString);
            return jsonObject;
        }
        public async Task<string> GetRedirect(string uri, Dictionary<string, string> headers = null)
        {
            var httpClientHandler = new HttpClientHandler { AllowAutoRedirect = false };

            var client = new HttpClient(httpClientHandler);

            client.DefaultRequestHeaders.Accept.Clear();
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
            var response = await client.GetAsync(uri);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return stringResponse;
        }
    }
}
