using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace PostgreSql.Controllers
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
        public IActionResult PostgresData(
                [FromServices] NpgsqlConnection dbConnection)
        {
            dbConnection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM TestData;", dbConnection);
            var rdr = cmd.ExecuteReader();

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
