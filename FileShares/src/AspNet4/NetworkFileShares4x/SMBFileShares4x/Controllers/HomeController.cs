using Microsoft.Extensions.Configuration;
using Steeltoe.Common.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SMBFileShares4x.Controllers
{
    public class HomeController : Controller
    {
        private readonly string demoFileName = "TextFileToTransfer.txt";
        private readonly string sharePath;
        private NetworkCredential ShareCredentials { get; set; }

        public HomeController()
        {
            var credHubEntry = ApplicationConfig.CloudFoundryServices.Services["credhub"].First(q => q.Name.Equals("steeltoe-network-share"));

            sharePath = credHubEntry.Credentials["location"].Value;
            var userName = credHubEntry.Credentials["username"].Value;
            var password = credHubEntry.Credentials["password"].Value;
            ShareCredentials = new NetworkCredential(userName, password);
            Console.WriteLine("Prepared to interact with the file share found at {0}", sharePath);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Copy()
        {
            using (var networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
            {
                System.IO.File.Copy(Path.Combine(Server.MapPath("~"), demoFileName), Path.Combine(sharePath, demoFileName), true);
            }

            return Json("File copied successfully", JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            try
            {
                using (var networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
                {
                    return Json(Directory.EnumerateFiles(sharePath), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public ActionResult Delete()
        {
            using (var networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
            {
                System.IO.File.Delete(Path.Combine(sharePath, demoFileName));
            }

            return Json("File deleted successfully", JsonRequestBehavior.AllowGet);
        }
    }
}