using VaccinationCard.Infrastructure;
using VaccinationCard.Application;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração de Serviços (DI)

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de Dependência
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

var app = builder.Build();

// 2. Configuração do Pipeline de Requisição

// Configura o Swagger UI no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();