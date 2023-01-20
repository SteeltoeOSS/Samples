using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Redis.Models;
using StackExchange.Redis;

namespace Redis.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
    {
        _logger = logger;
        _distributedCache = distributedCache;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public IActionResult Index()
    {
        // Steeltoe: Retrieve data from Redis cache.
        IDatabase database = _connectionMultiplexer.GetDatabase();

        var model = new RedisViewModel
        {
            CacheData =
            {
                ["Key1"] = GetValueFromDistributedCache("Key1"),
                ["Key2"] = GetValueFromDistributedCache("Key2")
            },
            MultiplexerData =
            {
                ["ConnectionMultiplexerKey1"] = GetValueFromConnectionMultiplexer(database, "ConnectionMultiplexerKey1"),
                ["ConnectionMultiplexerKey2"] = GetValueFromConnectionMultiplexer(database, "ConnectionMultiplexerKey2")
            },
            LuaResult = EvaluateLuaScript(database)
        };

        return View(model);
    }

    private string? GetValueFromDistributedCache(string keyName)
    {
        byte[]? value = _distributedCache.Get(keyName);
        return value != null ? Encoding.UTF8.GetString(value) : null;
    }

    private static string? GetValueFromConnectionMultiplexer(IDatabase database, string keyName)
    {
        RedisValue value = database.StringGet(keyName);
        return value.ToString();
    }

    private static string? EvaluateLuaScript(IDatabase database)
    {
        try
        {
            LuaScript? script = LuaScript.Prepare("local val=\"Hello from Lua\" return val");
            RedisResult? result = database.ScriptEvaluate(script);

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
