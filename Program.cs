using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carregar as variáveis do arquivo .env
Env.Load();

// Obter a chave secreta do ambiente
string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") 
    ?? throw new InvalidOperationException("SECRET_KEY não foi configurada.");

// Adiciona os serviços ao contêiner
builder.Services.AddControllers();

// Configura a autenticação JWT
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Defina sua chave secreta aqui
    };
});

// Adiciona OpenAPI (Swagger)
builder.Services.AddOpenApi();

var app = builder.Build();

// Configura o pipeline de requisições HTTP
app.UseAuthentication();  // Ativa a autenticação JWT
app.UseAuthorization();   // Ativa a autorização

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers(); // Mapeia os controllers da API

app.Run();


// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// builder.Services.AddControllers(); // Adiciona os controllers ao contêiner de serviços.
// builder.Services.AddOpenApi(); // Configura o OpenAPI (Swagger).

// // Configura a autenticação JWT
// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer(options =>
//     {
//         options.Authority = "https://localhost:5001"; // Aqui você deve colocar a URL de seu servidor de autenticação, caso tenha um
//         options.Audience = "productapi"; // O público para o qual o token foi emitido
//         options.RequireHttpsMetadata = false; // Defina como true para produção
//     });

// /*
// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Description = "JWT Authorization header using the Bearer scheme.",
//         Name = "Authorization",
//         In = ParameterLocation.Header,
//         Type = SecuritySchemeType.ApiKey
//     });

//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new string[] {}
//         }
//     });
// });
// */

// var app = builder.Build();

// // Ativa a autenticação JWT
// app.UseAuthentication();  // Ativa a autenticação
// app.UseAuthorization();   // Ativa a autorização

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi(); // Ativa o OpenAPI apenas no ambiente de desenvolvimento.
// }

// app.MapControllers(); // Mapeia os controllers da API.

// app.Run();
