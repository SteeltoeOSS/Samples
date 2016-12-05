using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySql4.Controllers
{
    public class HomeController : Controller
    {
        private MySqlConnection _dbConnection;
        public HomeController(MySqlConnection dbConnection)
        {
            _dbConnection = dbConnection;
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

        public ActionResult MySqlData()
        {
            _dbConnection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM TestData;", _dbConnection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ViewData["Key" + rdr[0]] = rdr[1];
            }

            rdr.Close();
            _dbConnection.Close();

            return View();
        }
    }
}