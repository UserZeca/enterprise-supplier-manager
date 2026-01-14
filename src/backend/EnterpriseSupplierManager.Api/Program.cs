using EnterpriseSupplierManager.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configuração de integração com a API de CEP
builder.Services.AddHttpClient("CepApi", c =>
{
    c.BaseAddress = new Uri("http://cep.la/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Aguarda um momento para o Docker terminar de subir o serviço de rede do banco
        context.Database.Migrate();
        Console.WriteLine(">>> Database synchronized successfully via Program.cs!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($">>> Error during migration: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
