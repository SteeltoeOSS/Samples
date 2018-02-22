using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace PostgreSql4.Controllers
{
    public class HomeController : Controller
    {
        private NpgsqlConnection _dbConnection;

        public HomeController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection as NpgsqlConnection;
        }

        public ActionResult Index()
        {
            var viewModel = new Dictionary<string, string>();
            _dbConnection.Open();

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM TestData;", _dbConnection);
            NpgsqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                viewModel.Add(rdr[0].ToString(), rdr[1].ToString());
            }

            ViewBag.Database = _dbConnection.Database;
            ViewBag.DataSource = _dbConnection.DataSource;
            rdr.Close();
            _dbConnection.Close();

            return View(viewModel);
        }
    }
}