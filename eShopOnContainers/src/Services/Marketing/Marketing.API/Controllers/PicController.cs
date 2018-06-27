namespace Microsoft.eShopOnContainers.Services.Marketing.API.Controllers
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.eShopOnContainers.Services.Marketing.API.Infrastructure;
  using System.IO;
  using System.Threading.Tasks;

  public class PicController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly MarketingContext _context;
        public PicController(MarketingContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        [Route("api/v1/campaigns/{campaignId:int}/pic")]
        public async Task<IActionResult>  GetImage(int campaignId)
        {
            var campaign = await _context.Campaigns
                .SingleOrDefaultAsync(c => c.Id == campaignId);

            if (campaign is null)
            {
                return NotFound();
            }

            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot, campaign.PictureName );

            var buffer = await System.IO.File.ReadAllBytesAsync(path); 
            
            return File(buffer, "image/png");
        }
    }
}
