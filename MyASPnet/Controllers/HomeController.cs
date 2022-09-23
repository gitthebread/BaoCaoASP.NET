using Microsoft.AspNetCore.Mvc;
using MyASPnet.Models;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace MyASPnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> list = new List<Product>();
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-SKTQ3SG\\NHATQUANG;Initial Catalog=mydatabase;Integrated Security=True");
            conn.Open();
            string query = "select * from Product";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Product product = new Product();
                product.Id = (int)reader["id"];
                product.Name = (string)reader["name"];
                product.Price = (string)reader["price"];
                product.Img = (string)reader["img"];
                product.Desc = (string)reader["description"];
                list.Add(product);
            }
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}