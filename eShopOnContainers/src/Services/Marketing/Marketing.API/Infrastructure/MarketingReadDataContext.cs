namespace Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure
{
  using System;
  using System.Linq;
  using Microsoft.eShopOnContainers.Services.Marketing.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
  using Steeltoe.Extensions.Configuration.CloudFoundry;

  public class MarketingReadDataContext
    {
        private readonly IMongoDatabase _database = null;

        public MarketingReadDataContext(IOptions<MarketingSettings> settings, IOptions<CloudFoundryServicesOptions> cloudFoundrySettings)
        {
            MongoClient client = null;
            var connectionString = settings.Value.MongoConnectionString;

            var service = cloudFoundrySettings.Value.ServicesList.FirstOrDefault(s=>s.Tags.Contains("mongodb"));
            if(service != null)
            {
                connectionString = service.Credentials.ContainsKey("uri") ? service.Credentials["uri"].Value: throw new Exception("No mongodb connection string");
                client = new MongoClient(connectionString);
                if (client != null)
                {
                    var databaseName = new UriBuilder(connectionString).Path.Replace("/",string.Empty);
                    _database = client.GetDatabase(databaseName);
                    return;
                }   
            }
            client = new MongoClient(connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.MongoDatabase);
            }
        }

        public IMongoCollection<MarketingData> MarketingData
        {
            get
            {
                return _database.GetCollection<MarketingData>("MarketingReadDataModel");
            }
        }        
    }
}
