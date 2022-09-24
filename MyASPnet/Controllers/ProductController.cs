using Microsoft.AspNetCore.Mvc;
using MyASPnet.Models;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace MyASPnet.Controllers
{
    public class ProductController : Controller
    {
        readonly string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["DefaultConnection"];
        public IActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pagesize = 3;
            int pagenumber = (page ?? 1);
            int from = (pagenumber - 1) * pagesize;
            List<Product> list = new List<Product>();

            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "select * from product where status = 1 order by id offset @from rows fetch next @size rows only";
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
            string query2 = "select count(*) from product where status = 1";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            object total = cmd2.ExecuteScalar();
            conn.Close();
            double result = Math.Ceiling((double)(int)total / pagesize);
            return (int)result;
        }
        public IActionResult Edit(int id) //Opening the edit page
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


        [HttpPost]
        public IActionResult Edit(Product product) //Editing the item
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

        public IActionResult Create() //Opening the create page
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product) //Creating the item
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string countquery = "select count(*) from product";
                    SqlCommand cmd2 = new SqlCommand(countquery, conn);
                    int count = (Int32)cmd2.ExecuteScalar();

                    string query = $"insert into Product values ({count + 1}, @name, @price, @img ,@desc ,1)";
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
            return View(product);
        }
    }
}
