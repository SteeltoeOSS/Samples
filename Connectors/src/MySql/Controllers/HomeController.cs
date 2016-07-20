using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MySql.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult MySqlData(
            [FromServices] MySqlConnection dbConnection)
        {
            dbConnection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM TestData;", dbConnection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ViewData["Key" + rdr[0]] = rdr[1];
            }

            rdr.Close();
            dbConnection.Close();

            return View();
        }
    }
}
