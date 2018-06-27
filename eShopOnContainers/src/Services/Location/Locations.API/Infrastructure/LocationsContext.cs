namespace Microsoft.eShopOnContainers.Services.Locations.API.Infrastructure
{
  using System;
  using System.Linq;
  using Microsoft.eShopOnContainers.Services.Locations.API.Model;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
  using Steeltoe.Extensions.Configuration.CloudFoundry;

  public class LocationsContext
    {
        private readonly IMongoDatabase _database = null;

        public LocationsContext(IOptions<LocationSettings> settings, IOptions<CloudFoundryServicesOptions> cloudFoundrySettings)
        {
            MongoClient client = null;
            var connectionString = settings.Value.ConnectionString;

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
                _database = client.GetDatabase(settings.Value.Database);
            }
        }

        public IMongoCollection<UserLocation> UserLocation
        {
            get
            {
                return _database.GetCollection<UserLocation>("UserLocation");
            }
        }

        public IMongoCollection<Locations> Locations
        {
            get
            {
                return _database.GetCollection<Locations>("Locations");
            }
        }       
    }
}
