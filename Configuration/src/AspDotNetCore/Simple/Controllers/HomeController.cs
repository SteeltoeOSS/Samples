using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Microsoft.Extensions.Configuration;
using Simple.Model;
using System;

namespace Simple.Controllers
{
    public class HomeController : Controller
    {
        private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }

        private ConfigServerClientSettingsOptions ConfigServerClientSettingsOptions { get; set; }

        private IConfigurationRoot Config { get; set; }

        public HomeController(IConfigurationRoot config, IOptionsSnapshot<ConfigServerData> configServerData, IOptions<ConfigServerClientSettingsOptions> confgServerSettings)
        {
            // The ASP.NET DI mechanism injects the data retrieved from the Spring Cloud Config Server 
            // as an IOptionsSnapshot<ConfigServerData>. This happens because we added the call to:
            // "services.Configure<ConfigServerData>(Configuration);" in the StartUp class
            if (configServerData != null)
                IConfigServerData = configServerData;

            // The settings used in communicating with the Spring Cloud Config Server
            if (confgServerSettings != null)
                ConfigServerClientSettingsOptions = confgServerSettings.Value;

            Config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult ConfigServer()
        {
            CreateConfigServerDataViewData();
            return View();
        }

        public IActionResult Reload()
        {
            if (Config != null)
            {
                Config.Reload();
            }

            return View();
        }

        private void CreateConfigServerDataViewData()
        {

            ViewData["ASPNETCORE_ENVIRONMENT"] = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // IConfigServerData property is set to a IOptionsSnapshot<ConfigServerData> that has been
            // initialized with the configuration data returned from the Spring Cloud Config Server
            if (IConfigServerData != null && IConfigServerData.Value != null)
            {
                var data = IConfigServerData.Value;
                ViewData["Bar"] = data.Bar ?? "Not returned";
                ViewData["Foo"] = data.Foo ?? "Not returned";

                ViewData["Info.Url"] = "Not returned";
                ViewData["Info.Description"] = "Not returned";

                if (data.Info != null)
                {
                    ViewData["Info.Url"] = data.Info.Url ?? "Not returned";
                    ViewData["Info.Description"] = data.Info.Description ?? "Not returned";
                }
            }
            else {
                ViewData["Bar"] = "Not Available";
                ViewData["Foo"] = "Not Available";
                ViewData["Info.Url"] = "Not Available";
                ViewData["Info.Description"] = "Not Available";
            }

        }

        public IActionResult ConfigServerSettings()
        {
            if (ConfigServerClientSettingsOptions != null)
            {
                ViewData["Enabled"] = ConfigServerClientSettingsOptions.Enabled;
                ViewData["Environment"] = ConfigServerClientSettingsOptions.Environment;
                ViewData["FailFast"] = ConfigServerClientSettingsOptions.FailFast;
                ViewData["Label"] = ConfigServerClientSettingsOptions.Label;
                ViewData["Name"] = ConfigServerClientSettingsOptions.Name;
                ViewData["Password"] = ConfigServerClientSettingsOptions.Password;
                ViewData["Uri"] = ConfigServerClientSettingsOptions.Uri;
                ViewData["Username"] = ConfigServerClientSettingsOptions.Username;
                ViewData["ValidateCertificates"] = ConfigServerClientSettingsOptions.ValidateCertificates;
            } else
            {

                ViewData["Enabled"] = "Not Available";
                ViewData["Environment"] = "Not Available";
                ViewData["FailFast"] = "Not Available";
                ViewData["Label"] = "Not Available";
                ViewData["Name"] = "Not Available";
                ViewData["Password"] = "Not Available";
                ViewData["Uri"] = "Not Available";
                ViewData["Username"] = "Not Available";
                ViewData["ValidateCertificates"] = "Not Available";
            }
            return View();
        }
    }
}
