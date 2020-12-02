using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreUI.Services
{
    public abstract class BaseDiscoveryService
    {
        protected HttpClient _client;
        protected ILogger _logger;

        public BaseDiscoveryService(HttpClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public virtual async Task<bool> Invoke(HttpRequestMessage request)
        {
            try
            {
                using var response = await _client.SendAsync(request);
                var stream = await response.Content.ReadAsStreamAsync();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }
        }
        public virtual async Task<bool> Invoke(HttpRequestMessage request, object content)
        {
            try
            {
                request.Content = Serialize(content);
                using var response = await _client.SendAsync(request);
                var stream = await response.Content.ReadAsStreamAsync();
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }
        }

        public virtual async Task<T> Invoke<T>(HttpRequestMessage request, object content = null)
        {
            try
            {
                if (content != null)
                {
                    request.Content = Serialize(content);
                }
                
                using var response = await _client.SendAsync(request);
                var stream = await response.Content.ReadAsStreamAsync();
                return Deserialize<T>(stream);
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }
        }

        public virtual T Deserialize<T>(Stream stream)
        {
            try
            {
                using JsonReader reader = new JsonTextReader(new StreamReader(stream));
                var serializer = new JsonSerializer();
                return (T)serializer.Deserialize(reader, typeof(T));
            }
            catch(Exception e)
            {
                _logger?.LogError("Deserialize exception: {0}", e);
                throw;
            }

        }

        public virtual HttpContent Serialize(object toSerialize)
        {
            var json = JsonConvert.SerializeObject(toSerialize);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
