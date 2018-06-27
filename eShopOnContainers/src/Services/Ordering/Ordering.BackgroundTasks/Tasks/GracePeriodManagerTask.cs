using Dapper;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Ordering.BackgroundTasks.Configuration;
using Ordering.BackgroundTasks.IntegrationEvents;
using Ordering.BackgroundTasks.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.BackgroundTasks.Tasks
{
    public class GracePeriodManagerService
         : BackgroundService
    {
        private readonly ILogger<GracePeriodManagerService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IEventBus _eventBus;
        private readonly MySqlConnection _dbConnection;

        public GracePeriodManagerService(IOptions<BackgroundTaskSettings> settings,
                                         IEventBus eventBus,
                                         ILogger<GracePeriodManagerService> logger,
                                         MySqlConnection dbConnection)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug($"#1 GracePeriodManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GracePeriodManagerService background task is doing background work.");

                CheckConfirmedGracePeriodOrders();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug($"GracePeriodManagerService background task is stopping.");

            await Task.CompletedTask;
        }

        private void CheckConfirmedGracePeriodOrders()
        {
            _logger.LogDebug($"Checking confirmed grace period orders");

            var orderIds = GetConfirmedGracePeriodOrders();

            foreach (var orderId in orderIds)
            {
                var confirmGracePeriodEvent = new GracePeriodConfirmedIntegrationEvent(orderId);

                _eventBus.Publish(confirmGracePeriodEvent);
            }
        }

        private IEnumerable<int> GetConfirmedGracePeriodOrders()
        {
            IEnumerable<int> orderIds = new List<int>();
            
            try
            {
                if(_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();
                    
                orderIds = _dbConnection.Query<int>(
                    @"SELECT Id FROM [orders] 
                        WHERE DATEDIFF(minute, [OrderDate], GETDATE()) >= @GracePeriodTime
                        AND [OrderStatusId] = 1",
                    new { GracePeriodTime = _settings.GracePeriodTime });
            }
            catch (SqlException exception)
            {
                _logger.LogCritical($"FATAL ERROR: Database connections could not be opened: {exception.Message}");
            }
            return orderIds;
        }
    }
}
