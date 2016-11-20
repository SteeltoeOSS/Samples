using AutofacCloudFoundry.Models;
using AutofacCloudFoundry.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Web.Mvc;

namespace AutofacCloudFoundry.Controllers
{
    public class HomeController : Controller
    {
        private IOptionsSnapshot<ConfigServerData> IConfigServerData { get; set; }
        private CloudFoundryServicesOptions CloudFoundryServices { get; set; }
        private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }
        private ConfigServerClientSettingsOptions ConfigServerClientSettingsOptions { get; set; }
        private IConfigurationRoot Config { get; set; }

        public HomeController(IConfigurationRoot config,
            IOptionsSnapshot<ConfigServerData> configServerData,
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> servOptions,
            IOptions<ConfigServerClientSettingsOptions> confgServerSettings)
        {
            // The Autofac DI mechanism injects the data retrieved from the
            // Spring Cloud Config Server as an IOptionsSnapshot<ConfigServerData>
            // since we added "builder.Configure<ConfigServerData>(Configuration);"
            // in the Global startup class
            if (configServerData != null)
                IConfigServerData = configServerData;

            // The Autofac DI mechanism injects these as well, see RegisterConfigServer
            if (servOptions != null)
                CloudFoundryServices = servOptions.Value;
            if (appOptions != null)
                CloudFoundryApplication = appOptions.Value;

            // Injected the settings used in communicating with the Spring Cloud Config Server
            if (confgServerSettings != null)
                ConfigServerClientSettingsOptions = confgServerSettings.Value;

            Config = config;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ConfigServerSettings()
        {

            if (ConfigServerClientSettingsOptions != null)
            {

                ViewBag.Enabled = ConfigServerClientSettingsOptions.Enabled;
                ViewBag.Environment = ConfigServerClientSettingsOptions.Environment;
                ViewBag.FailFast = ConfigServerClientSettingsOptions.FailFast;
                ViewBag.Label = ConfigServerClientSettingsOptions.Label;
                ViewBag.Name = ConfigServerClientSettingsOptions.Name;
                ViewBag.Password = ConfigServerClientSettingsOptions.Password;
                ViewBag.Uri = ConfigServerClientSettingsOptions.Uri;
                ViewBag.Username = ConfigServerClientSettingsOptions.Username;
                ViewBag.ValidateCertificates = ConfigServerClientSettingsOptions.ValidateCertificates;
                ViewBag.AccessTokenUri = ConfigServerClientSettingsOptions.AccessTokenUri;
                ViewBag.ClientId = ConfigServerClientSettingsOptions.ClientId; ;
                ViewBag.ClientSecret = ConfigServerClientSettingsOptions.ClientSecret;
            }
            else
            {

                ViewBag.Enabled = "Not Available";
                ViewBag.Environment = "Not Available";
                ViewBag.FailFast = "Not Available";
                ViewBag.Label = "Not Available";
                ViewBag.Name = "Not Available";
                ViewBag.Password = "Not Available";
                ViewBag.Uri = "Not Available";
                ViewBag.Username = "Not Available";
                ViewBag.ValidateCertificates = "Not Available";
                ViewBag.AccessTokenUri = "Not Available";
                ViewBag.ClientId = "Not Available";
                ViewBag.ClientSecret = "Not Available";
            }
            return View();
        }

        public ActionResult ConfigServerData()
        {

            CreateConfigServerDataViewData();
            return View();
        }

        public ActionResult CloudFoundry()
        {
            return View(new CloudFoundryViewModel(
                CloudFoundryApplication == null ? new CloudFoundryApplicationOptions() : CloudFoundryApplication,
                CloudFoundryServices == null ? new CloudFoundryServicesOptions() : CloudFoundryServices));
        }

        public ActionResult Reload()
        {
            if (Config != null)
            {
                Config.Reload();
            }

            return View();
        }

        private void CreateConfigServerDataViewData()
        {

            // IConfigServerData property is set to a IOptionsSnapshot<ConfigServerData> that has been
            // initialized with the configuration data returned from the Spring Cloud Config Server
            if (IConfigServerData != null && IConfigServerData.Value != null)
            {
                var data = IConfigServerData.Value;
                ViewBag.Bar = data.Bar ?? "Not returned";
                ViewBag.Foo = data.Foo ?? "Not returned";

                ViewBag.Info_Url = "Not returned";
                ViewBag.Info_Description = "Not returned";

                if (data.Info != null)
                {
                    ViewBag.Info_Url = data.Info.Url ?? "Not returned";
                    ViewBag.Info_Description = data.Info.Description ?? "Not returned";
                }
            }
            else
            {
                ViewBag.Bar = "Not Available";
                ViewBag.Foo = "Not Available";
                ViewBag.Info_Url = "Not Available";
                ViewBag.Info_Description = "Not Available";
            }

        }
    }
}