using Autofac;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.MySql;
using Steeltoe.CloudFoundry.Connector.Services;


namespace MySql4
{
    public static class MySqlContainerBuilderExtensions
    {
        public static void RegisterMySqlConnection(this ContainerBuilder container, IConfigurationRoot config)
        {
            MySqlProviderConnectorOptions mySqlConfig = new MySqlProviderConnectorOptions(config);
            MySqlServiceInfo info = config.GetSingletonServiceInfo<MySqlServiceInfo>();
            MySqlProviderConnectorFactory factory = new MySqlProviderConnectorFactory(info, mySqlConfig, typeof(MySqlConnection));
            container.Register<MySqlConnection>(c => (MySqlConnection)factory.Create(null)).InstancePerLifetimeScope();
        }
    }
}