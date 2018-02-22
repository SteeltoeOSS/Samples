using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Mvc;

namespace MySql4.Controllers
{
    public class HomeController : Controller
    {
        private IDbConnection _dbConnection;

        public HomeController(IDbConnection dbConnection)
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

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM TestData;", (MySqlConnection)_dbConnection);
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