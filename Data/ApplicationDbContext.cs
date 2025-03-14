using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Inicializar explicitamente as propriedades não anuláveis
            Products = Set<Product>();
            Users = Set<User>();
        }

        // Propriedades DbSet para as tabelas "Products" e "Users"
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
