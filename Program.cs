using Microsoft.EntityFrameworkCore;
using MotoScan.Data;
using MotoScan.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicionar os servi�os
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura��o do DbContext Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Registrar o ImagemService
builder.Services.AddScoped<ImagemService>();

var app = builder.Build();

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Inicializar o banco de dados com dados de teste
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            // Como ILogger<Program> pode n�o estar dispon�vel, use Console
            Console.Error.WriteLine($"Ocorreu um erro ao inicializar o banco de dados: {ex.Message}");
        }
    }
}

// Habilitar arquivos est�ticos (para as imagens)
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();