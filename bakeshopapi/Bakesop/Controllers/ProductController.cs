using Bakesop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bakesop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // In-memory list (temporary database)
        private static List<Products> products = new List<Products>()
        {
            new Products
            {
                Id = 1,
                Name = "Chocolate Cake",
                Price = 1500,
                ImageUrl = "https://example.com/chocolate-cake.jpg"
            },
            new Products
            {
                Id = 2,
                Name = "Cup Cake",
                Price = 300,
                ImageUrl = "https://example.com/cup-cake.jpg"
            }
        };

        // READ ALL
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(products);
        }

        // READ BY ID
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        // CREATE
        [HttpPost]
        public IActionResult AddProduct(Products product)
        {
            product.Id = products.Any() ? products.Max(p => p.Id) + 1 : 1;
            products.Add(product);

            return CreatedAtAction(nameof(GetProductById),
                new { id = product.Id }, product);
        }

        // UPDATE
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Products updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.ImageUrl = updatedProduct.ImageUrl;

            return Ok(product);
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            products.Remove(product);
            return NoContent();
        }
    }
}
