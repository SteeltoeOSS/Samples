using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steeltoe.Samples.FortuneTellerApi.Data;
using Steeltoe.Samples.FortuneTellerApi.Models;

namespace Steeltoe.Samples.FortuneTellerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FortunesController(FortuneDbContext dbContext, ILogger<FortunesController> logger) : ControllerBase
{
    private readonly FortuneDbContext _dbContext = dbContext;
    private readonly ILogger<FortunesController> _logger = logger;

    [HttpGet]
    public async Task<ICollection<Fortune>> GetAll()
    {
        Fortune[] fortunes = await _dbContext.Fortunes.OrderBy(fortune => fortune.Id).ToArrayAsync();

        _logger.LogInformation("GET api/fortunes: returning {Count} entries.", fortunes.Length);
        return fortunes;
    }

    [HttpGet("random")]
    public async Task<Fortune?> Random()
    {
        int count = await _dbContext.Fortunes.CountAsync();
        int index = System.Random.Shared.Next(0, count);
        Fortune? fortune = await _dbContext.Fortunes.OrderBy(fortune => fortune.Id).Skip(index).Take(1).SingleOrDefaultAsync();

        if (fortune == null)
        {
            _logger.LogInformation("GET api/fortunes/random: no entries found.");
        }
        else
        {
            _logger.LogInformation("GET api/fortunes/random: returning entry at index {Index} from {Count} entries.", index, count);
        }

        return fortune;
    }
}
