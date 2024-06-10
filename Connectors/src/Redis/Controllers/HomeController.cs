using System.Diagnostics;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Models;
using StackExchange.Redis;
using Steeltoe.Connectors;
using Steeltoe.Connectors.Redis;

namespace Redis.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Connector<RedisOptions, IDistributedCache> _distributedCacheConnector;
    private readonly Connector<RedisOptions, IConnectionMultiplexer> _connectionMultiplexerConnector;

    public HomeController(ILogger<HomeController> logger, ConnectorFactory<RedisOptions, IDistributedCache> distributedCacheConnectorFactory,
        ConnectorFactory<RedisOptions, IConnectionMultiplexer> connectionMultiplexerConnectorFactory)
    {
        _logger = logger;
        _distributedCacheConnector = distributedCacheConnectorFactory.Get();
        _connectionMultiplexerConnector = connectionMultiplexerConnectorFactory.Get();
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Steeltoe: Retrieve data from Redis cache. Do not dispose the IConnectionMultiplexer singleton.
        IConnectionMultiplexer connectionMultiplexer = _connectionMultiplexerConnector.GetConnection();
        IDatabase database = connectionMultiplexer.GetDatabase();
        List<string> keyNames = await GetKeyNamesAsync(connectionMultiplexer, cancellationToken);

        var model = new RedisViewModel
        {
            ConnectionString = _connectionMultiplexerConnector.Options.ConnectionString,
            DistributedCacheData = new Dictionary<string, string?>(),
            ConnectionMultiplexerData = new Dictionary<string, string?>(),
            LuaResult = EvaluateLuaScript(database)
        };

        foreach (string keyName in keyNames.OrderBy(name => name))
        {
            string? valueFromDistributedCache = await GetValueFromDistributedCacheAsync(connectionMultiplexer.ClientName, keyName, cancellationToken);
            model.DistributedCacheData.Add(keyName, valueFromDistributedCache);

            string? valueFromConnectionMultiplexer = await GetValueFromConnectionMultiplexerAsync(database, keyName);
            model.ConnectionMultiplexerData.Add(keyName, valueFromConnectionMultiplexer);
        }

        return View(model);
    }

    private static async Task<List<string>> GetKeyNamesAsync(IConnectionMultiplexer connectionMultiplexer, CancellationToken cancellationToken)
    {
        EndPoint endPoint = connectionMultiplexer.GetEndPoints().First();
        IServer server = connectionMultiplexer.GetServer(endPoint);

        var keyNames = new List<string>();

        await foreach (string? keyName in server.KeysAsync().WithCancellation(cancellationToken))
        {
            if (keyName != null)
            {
                keyNames.Add(keyName);
            }
        }

        return keyNames;
    }

    private async Task<string?> GetValueFromDistributedCacheAsync(string instanceName, string keyName, CancellationToken cancellationToken)
    {
        IDistributedCache distributedCache = _distributedCacheConnector.GetConnection();

        string appKeyName = keyName.StartsWith(instanceName, StringComparison.Ordinal) ? keyName[instanceName.Length..] : keyName;
        byte[]? value = await distributedCache.GetAsync(appKeyName, cancellationToken);
        return value != null ? Encoding.UTF8.GetString(value) : null;
    }

    private static async Task<string?> GetValueFromConnectionMultiplexerAsync(IDatabase database, string keyName)
    {
        RedisValue value = await database.HashGetAsync(keyName, "data");
        return value.ToString();
    }

    private static string? EvaluateLuaScript(IDatabase database)
    {
        try
        {
            LuaScript script = LuaScript.Prepare("local val=\"Hello from Lua\" return val");
            RedisResult result = database.ScriptEvaluate(script);

            return result.ToString();
        }
        catch
        {
            return "Failed to execute Lua script, scripting is likely not enabled.";
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
