using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data; // Adiciona a referência para o ApplicationDbContext
using System.Threading.Tasks;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Chave secreta
        private static readonly string SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY") 
            ?? throw new InvalidOperationException("SECRET_KEY não foi configurada.");
        private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        // Injeta o contexto do banco de dados
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint para login (gerar o JWT)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // Verificar se o usuário existe no banco de dados
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized(); // Retorna 401 se o login falhar
            }

            // Cria as claims para o JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),  // Define um papel (Role) para o usuário (pode ser admin, user, etc)
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
