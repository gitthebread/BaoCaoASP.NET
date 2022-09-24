using Microsoft.AspNetCore.Mvc;
using MyASPnet.Models;
using System.Data.SqlClient;

namespace MyASPnet.Controllers
{
    public class SearchController : Controller
    {
        readonly string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchResult(string searchstr)
        {
            List<Product> list = new List<Product>();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = $"SELECT * from Product WHERE name LIKE '%{searchstr.ToLower()}%' and status = 1";

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
            conn.Close();
            return View(list);
        }
    }
}
