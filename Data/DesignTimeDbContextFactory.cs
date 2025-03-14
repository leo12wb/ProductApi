using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductApi.Data;
using System.IO;

namespace ProductApi.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Caminho base para o diretório atual
                .AddJsonFile("appsettings.json")              // Carregar configurações do appsettings.json
                .Build();

            // Obter a string de conexão do arquivo de configuração
            var connectionString = configuration.GetConnectionString("MySqlConnection") ??
                throw new InvalidOperationException("A string de conexão do MySQL não foi configurada.");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
