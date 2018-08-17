using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Steeltoe.Common.Discovery;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public abstract class AbstractService 
    {
        protected DiscoveryHttpClientHandler _handler;
        protected ILogger<AbstractService> _logger;
        protected IHttpContextAccessor _context;

        public AbstractService(IDiscoveryClient client, ILogger<AbstractService> logger, IHttpContextAccessor context)
        {
            _handler = new DiscoveryHttpClientHandler(client)
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _logger = logger;
            _context = context;
        }
        public async Task DoRequest(HttpClient client, HttpRequestMessage request)
        {
  
            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return;

                    // Log status
                    var message = string.Format("Service request returned status: {0} invoking path: {1}",
                        response.StatusCode, request.RequestUri);

                    _logger?.LogInformation(message);

                    return;
                }
                return;
            }
    
        }

        public async Task<T> DoRequest<T>(HttpClient client, HttpRequestMessage request)
        {

            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return default(T);

                    // Log status
                    var message = string.Format("Service request returned status: {0} invoking path: {1}",
                        response.StatusCode, request.RequestUri);

                    _logger?.LogInformation(message);

                    return default(T);
                }

                Stream stream = await response.Content.ReadAsStreamAsync();
                return Deserialize<T>(stream);
            }

        }
        public async Task<T> DoHateoasRequest<T>(HttpClient client, HttpRequestMessage request, string key)
        {


            using (HttpResponseMessage response = await client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return default(T);

                    // Log status
                    var message = string.Format("Service request returned status: {0} invoking path: {1}",
                        response.StatusCode, request.RequestUri);

                    _logger?.LogInformation(message);

                    return default(T);
                }

                string json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json))
                {
                    return default(T);
                }
                var parsed = JObject.Parse(json);
                if (parsed == null)
                {
                    return default(T);
                }
                var items = parsed["_embedded"]?[key];
                if (items == null)
                {
                    return default(T);
                }
                return items.ToObject<T>();

                //return Deserialize<T>(stream);
            }

        }
        protected virtual T Deserialize<T>(Stream stream)
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (T)serializer.Deserialize(reader, typeof(T));
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Serialization exception: {0}", e);
            }
            return default(T);
        }

        protected virtual HttpRequestMessage GetRequest(HttpMethod method, string requestUri)
        {
            var request = new HttpRequestMessage(method, requestUri);
      
            return request;
        }

        protected virtual async Task<HttpClient> GetClientAsync()
        {
            var client = new HttpClient(_handler, false);

            var token = await _context.HttpContext.GetTokenAsync("access_token");
            if (token != null)
            {
                _logger.LogDebug("Found a token {token}", token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _logger.LogDebug("Token not found in HttpContext");
                string authHeader = await Task.FromResult(_context.HttpContext.Request.Headers["Authorization"]);
                if (authHeader != null)
                {
                    _logger.LogDebug("Found authorization header");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("bearer ", string.Empty));
                }
            }

            return client;
        }

        protected virtual HttpContent GetRequestContent(object toSerialize)
        {
            try
            {
                string json = JsonConvert.SerializeObject(toSerialize);
                _logger?.LogDebug("GetRequestContent generated JSON: {0}", json);
                return new StringContent(json, Encoding.UTF8, "application/json");

            }
            catch (Exception e)
            {
                _logger?.LogError("GetRequestContent Exception: {0}", e);
            }

            return new StringContent(string.Empty, Encoding.UTF8, "application/json");
        }
    }
}
