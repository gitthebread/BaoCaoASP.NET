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
        string connectionString = "Data Source=DESKTOP-2TS7TPE\\NHATQUANG;Initial Catalog=mydatabase;Integrated Security=True";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> list = new List<Product>();

            SqlConnection conn = new SqlConnection(connectionString);
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
        [HttpPost]
        public IActionResult Create()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "update product set name = @name , price = @price , img = @img , description = @desc where id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Product product = new Product();
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select * from product where id = @Id";
                SqlDataAdapter sqlda = new SqlDataAdapter(query, conn);
                sqlda.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlda.Fill(dt);
            }
            if (dt.Rows.Count == 1)
            {
                product.Id = Convert.ToInt32(dt.Rows[0][0]);
                product.Name = (string)dt.Rows[0][1];
                product.Price = (string)dt.Rows[0][2];
                product.Img = (string)dt.Rows[0][3];
                product.Desc = (string)dt.Rows[0][4];
                return View(product);
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "update product set name = @name , price = @price , img = @img , description = @desc where id = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@img", product.Img);
                cmd.Parameters.AddWithValue("@desc", product.Desc);
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
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