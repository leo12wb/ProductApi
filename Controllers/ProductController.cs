using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;  // Importa a namespace para o atributo [Authorize]

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Produto 1", Description = "Descrição do Produto 1", Price = 10.99m },
            new Product { Id = 2, Name = "Produto 2", Description = "Descrição do Produto 2", Price = 20.99m }
        };

        // GET: api/products
        [HttpGet]
        [Authorize]  // Protege a rota com autenticação JWT
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(products);
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {

            // Verificar se o nome é válido (não nulo ou vazio)
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("O nome do produto não pode ser vazio.");
            }

            // Verificar se o nome tem mais de 3 caracteres
            if (product.Name.Length <= 3)
            {
                return BadRequest("O nome do produto deve ter mais de 3 caracteres.");
            }

            // Verificar se o preço é válido (não zero ou negativo)
            if (product.Price <= 0)
            {
                return BadRequest("O preço do produto deve ser maior que zero.");
            }

            product.Id = products.Max(p => p.Id) + 1; // Gera um ID único
            products.Add(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            var existingProduct = products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Validação do Name
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return BadRequest("O nome do produto não pode ser vazio.");
            }

            if (product.Name.Length <= 3)
            {
                return BadRequest("O nome do produto deve ter mais de 3 caracteres.");
            }

            // Validação do Price
            if (product.Price <= 0)
            {
                return BadRequest("O preço do produto deve ser maior que zero.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            // return NoContent(); // Não retorna nada, mas indica sucesso
            // Retorna o produto atualizado com status 200 OK
            return Ok(existingProduct);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            products.Remove(product);

            return NoContent(); // Indica que o produto foi removido com sucesso
        }
    }
}
