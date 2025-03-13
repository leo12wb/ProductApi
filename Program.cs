using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d2f3f8c9e7b1a7b6e5c3a4d9b9c2e8e9f9d5d6a3c4f7f9f6b8f5e7d2a9d3e7d")) // Defina sua chave secreta aqui
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
