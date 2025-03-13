var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Adiciona os controllers ao contêiner de serviços.
builder.Services.AddOpenApi(); // Configura o OpenAPI (Swagger).

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Ativa o OpenAPI apenas no ambiente de desenvolvimento.
}

app.MapControllers(); // Mapeia os controllers da API.

app.Run();
