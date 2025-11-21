using VaccinationCard.Infrastructure;
using VaccinationCard.Application;
using VaccinationCard.Infrastructure.Persistence;

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


// 2. Bloco de SEEDING para criar vacinas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<VaccinationDbContext>();
        // Esta linha chama a classe que cria as vacinas
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro ao popular o banco de dados.");
    }
}

// 3. Configuração do Pipeline de Requisição

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