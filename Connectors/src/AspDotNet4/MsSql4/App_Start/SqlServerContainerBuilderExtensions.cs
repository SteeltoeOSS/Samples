using Microsoft.Extensions.Configuration;
using MsSql4.Data;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.CloudFoundry.Connector.SqlServer;
using StructureMap;
using System.Data.SqlClient;

namespace MsSql4
{
    public static class SqlServerContainerBuilderExtensions
    {
        public static void RegisterSqlServerConnection(IContainer container, IConfigurationRoot config)
        {
            SqlServerProviderConnectorOptions SqlServerConfig = new SqlServerProviderConnectorOptions(config);
            SqlServerServiceInfo info = config.GetSingletonServiceInfo<SqlServerServiceInfo>();
            SqlServerProviderConnectorFactory factory = new SqlServerProviderConnectorFactory(info, SqlServerConfig, typeof(SqlConnection));
            container.Inject<IBloggingContext>(new BloggingContext((SqlConnection)factory.Create(null)));
        }
    }
}