
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.Models;
using Common.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace AdminPortal.Controllers
{
    [Authorize(Policy = "MenuWrite")]
    public class HomeController : Controller
    {
        private Branding _branding;
        private IMenuService _menuService;
        private IOrderService _orderService;
        private ILogger<HomeController> _logger;

        public HomeController(IOptions<Branding> branding, IMenuService menuService, IOrderService orderService, ILoggerFactory fact)
        {
            _branding = branding.Value;
            _logger = fact.CreateLogger<HomeController>();
            _menuService = menuService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            ViewData["username"] = this.HttpContext.User.Identity.Name;
            ViewData["restaurantName"] = _branding.RestaurantName;
            return View();
        }
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            await _orderService.RemoveOrderAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MenuItems()
        {
            ViewData["menuTitle"] = _branding.MenuTitle;
            var result = await _menuService.GetMenuItemsAsync();
            return View(result);
        }

        public async Task<IActionResult> MenuItem(long id)
        {
            ViewData["menuTitle"] = _branding.MenuTitle;
            if (id >= 0)
            {
                var result = await _menuService.GetMenuItemAsync(id);
                return View(result);
            } else
            {
                return View(new MenuItem());
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMenuItem(long id, MenuItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _logger.LogInformation(string.Format("MenuItem: {0}, {1}, {2}", item.Id, item.Name, item.Price));
            if (id == -1)
            {
                await _menuService.SaveMenuItemAsync(item, true);
                return RedirectToAction("Index");
            }
            else
            {
                await _menuService.SaveMenuItemAsync(item, false);
                return RedirectToAction("Index");
            }


        }

        [HttpPost]
        public async Task<IActionResult> DeleteMenuItem(long id)
        {
            await _menuService.DeleteMenuItemAsync(id);
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> ViewToken()
        {
            ViewData["token"] = await this.HttpContext.Authentication.GetTokenAsync("access_token");
            return View();
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewData["Message"] = "Insufficient permissions.";
            return View();
        }
    }
}
