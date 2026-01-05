using Bakesop.DAL;
using Bakesop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace Bakesop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsSqliteController : ControllerBase
    {
        // GET ALL
        [HttpGet]
        public ActionResult<List<Products>> GetAllProducts()
        {
            List<Products> products = new();

            using var conn = Database.GetConnection();
            string query = "SELECT Id, Name, Price, ImageUrl FROM Products";
            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Products
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    ImageUrl = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }

            return products;
        }

        // GET BY ID
        [HttpGet("{id}")]
        public ActionResult<Products> GetProduct(int id)
        {
            using var conn = Database.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
                @"SELECT Id, Name, Price, ImageUrl
                  FROM Products WHERE Id = $id";

            cmd.Parameters.AddWithValue("$id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound();

            return new Products
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDouble(2),
                ImageUrl = reader.IsDBNull(3) ? null : reader.GetString(3)
            };
        }

        // CREATE
        [HttpPost]
        public ActionResult<int> AddProduct(Products product)
        {
            using var conn = Database.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
                @"INSERT INTO Products (Name, Price, ImageUrl)
                  VALUES ($name, $price, $imageUrl)";

            cmd.Parameters.AddWithValue("$name", product.Name);
            cmd.Parameters.AddWithValue("$price", product.Price);
            cmd.Parameters.AddWithValue("$imageUrl", product.ImageUrl);

            int result = cmd.ExecuteNonQuery();
            return result;
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Products product)
        {
            if (id != product.Id)
                return BadRequest();

            using var conn = Database.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText =
                @"UPDATE Products
                  SET Name=$name, Price=$price, ImageUrl=$imageUrl
                  WHERE Id=$id";

            cmd.Parameters.AddWithValue("$id", product.Id);
            cmd.Parameters.AddWithValue("$name", product.Name);
            cmd.Parameters.AddWithValue("$price", product.Price);
            cmd.Parameters.AddWithValue("$imageUrl", product.ImageUrl);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            using var conn = Database.GetConnection();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM Products WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return NoContent();
        }
    }
}
