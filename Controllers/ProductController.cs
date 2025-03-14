using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;  // Importa a namespace para o atributo [Authorize]
using ProductApi.Data;  // Adiciona a referência para o DbContext

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Construtor que injeta o DbContext
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        [Authorize]  // Protege a rota com autenticação JWT
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Retorna a lista de produtos do banco de dados
            return Ok(await _context.Products.ToListAsync());
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
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

            // Adiciona o produto ao banco de dados
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PUT: api/products/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var existingProduct = await _context.Products.FindAsync(id);

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

            await _context.SaveChangesAsync();

            return Ok(existingProduct);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica que o produto foi removido com sucesso
        }
    }
}
