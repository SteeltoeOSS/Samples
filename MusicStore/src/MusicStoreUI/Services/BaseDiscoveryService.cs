using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;
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
        protected DiscoveryHttpClientHandler _handler;
        protected ILogger _logger;

        public BaseDiscoveryService(IDiscoveryClient client, ILogger logger)
        {
            _handler = new DiscoveryHttpClientHandler(client, logger);
            _logger = logger;
        }
        public virtual HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
        public virtual async Task<bool> Invoke(HttpRequestMessage request)
        {
            var client = GetClient();
            try
            {
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }

            //return false;
        }
        public virtual async Task<bool> Invoke(HttpRequestMessage request, object content)
        {
            var client = GetClient();
            try
            {
                request.Content = Serialize(content);
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }

            //return false;
        }

        public virtual async Task<T> Invoke<T>(HttpRequestMessage request, object content = null)
        {
            var client = GetClient();
            try
            {
                if (content != null)
                {
                    request.Content = Serialize(content);
                }
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return Deserialize<T>(stream);
                }
            }
            catch (Exception e)
            {
                _logger?.LogError("Invoke exception: {0}", e);
                throw;
            }

            //return default(T);
        }

        public virtual T Deserialize<T>(Stream stream)
        {
            try
            {
                using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return (T)serializer.Deserialize(reader, typeof(T));
                }
            } catch(Exception e)
            {
                _logger?.LogError("Deserialize exception: {0}", e);
                throw;
            }

        }

        public virtual HttpContent Serialize(object toSerialize)
        {
            string json = JsonConvert.SerializeObject(toSerialize);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
