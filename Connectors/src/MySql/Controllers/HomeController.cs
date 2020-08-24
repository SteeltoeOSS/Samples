using System.Collections.Generic;
using System.Data.Common;
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

        public IActionResult MySqlData([FromServices] MySqlConnection dbConnection)
        {
            var viewData = new Dictionary<string, string>();
            dbConnection.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM TestData;", dbConnection);
            DbDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                viewData.Add(rdr[0].ToString(), rdr[1].ToString());
            }
            ViewBag.Database = dbConnection.Database;
            ViewBag.DataSource = dbConnection.DataSource;

            dbConnection.Close();

            return View(viewData);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
