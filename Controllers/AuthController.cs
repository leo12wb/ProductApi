using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Chave secreta
        private static readonly string SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY") 
            ?? throw new InvalidOperationException("SECRET_KEY não foi configurada.");
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        // Simulação de um "banco de dados" de usuários
        private static readonly User[] Users = new[]
        {
            new User { Email = "user@example.com", Password = "password123" },
            new User { Email = "admin@example.com", Password = "adminpassword" },
        };

        // Endpoint para login (gerar o JWT)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Verificar se o usuário existe no "banco de dados"
            var user = Users.FirstOrDefault(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
            if (user == null)
            {
                return Unauthorized(); // Retorna 401 se o login falhar
            }

            // Cria as claims para o JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Email.Contains("admin") ? "Admin" : "User"),
            };

            // Cria o token JWT
            var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: credentials
            );

            // Retorna o token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
