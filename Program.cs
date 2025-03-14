using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Data;
using Microsoft.OpenApi.Models;
using DotNetEnv;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Carregar as variáveis do arquivo .env (se necessário)
Env.Load();

// Configurar DbContext para MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySqlConnection"), 
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySqlConnection"))));


// Configuração de autenticação JWT
string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY")
    ?? throw new InvalidOperationException("SECRET_KEY não foi configurada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Chave secreta
    };
});

// Adicionar controllers
builder.Services.AddControllers();

var app = builder.Build();

// Ativar a autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Configurar o pipeline de requisições HTTP
app.MapControllers();

app.Run();
