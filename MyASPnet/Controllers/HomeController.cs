using Microsoft.AspNetCore.Mvc;
using MyASPnet.Models;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.Versioning;

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

        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pagesize = 3;
            int pagenumber = (page ?? 1);
            int from = (pagenumber - 1) * pagesize;
            List<Product> list = new List<Product>();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "select * from product order by id offset @from rows fetch next @size rows only";
            List<SqlParameter> listpara = new List<SqlParameter>()
            {
                new SqlParameter("@from", from),
                new SqlParameter("@size", pagesize),

            };
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddRange(listpara.ToArray());
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
            return View(new Pagination(list, totalPage(pagesize)));
        }
        private int totalPage(int pagesize)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query2 = "select count(*) from product";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            object total = cmd2.ExecuteScalar();
            conn.Close();
            double result = Math.Ceiling((double)(int)total / pagesize);
            return (int)result;
        }
        public IActionResult Edit(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "select * from product where id = @id";
            List<SqlParameter> listpara = new List<SqlParameter>()
            {
                new SqlParameter("@id", id),
            };
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddRange(listpara.ToArray());
            SqlDataReader reader = cmd.ExecuteReader();
            Product product = new Product();
            while (reader.Read())
            {

                product.Id = (int)reader["id"];
                product.Name = (string)reader["name"];
                product.Price = (string)reader["price"];
                product.Img = (string)reader["img"];
                product.Desc = (string)reader["description"];

            }
            conn.Close();
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            return View();
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