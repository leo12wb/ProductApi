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
        private const string SECRET_KEY = "d2f3f8c9e7b1a7b6e5c3a4d9b9c2e8e9f9d5d6a3c4f7f9f6b8f5e7d2a9d3e7d"; // 256 bits = 32 caracteres

        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        // Endpoint para login (gerar o JWT)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Validação básica de usuário e senha. Em um cenário real, você faria isso em um banco de dados.
            if (loginRequest.Email == "user@example.com" && loginRequest.Password == "password123")
            {
                // Cria as claims para o JWT
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, loginRequest.Email),
                    new Claim(ClaimTypes.Role, "User"),
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

            return Unauthorized(); // Retorna 401 se o login falhar
        }
    }

    // Classe para os dados de login (email e senha)
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
