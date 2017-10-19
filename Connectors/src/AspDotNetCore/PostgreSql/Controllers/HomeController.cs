using System.Collections.Generic;
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

        public IActionResult PostgresData([FromServices] NpgsqlConnection dbConnection)
        {
            var viewData = new Dictionary<string, string>();
            dbConnection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM TestData;", dbConnection);
            var rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                viewData.Add(rdr[0].ToString(), rdr[1].ToString());
            }
            ViewBag.Database = dbConnection.Database;
            ViewBag.DataSource = dbConnection.DataSource;
            rdr.Close();
            dbConnection.Close();
            return View(viewData);
        }
    }
}
