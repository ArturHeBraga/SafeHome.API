using SafeHome;
using SafeHome.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SafeHome API", Version = "v1" });
});
builder.Services.AddSingleton<MongoDbService>();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Testando a conexão com o MongoDB
var mongoDbService = app.Services.GetRequiredService<MongoDbService>();
mongoDbService.TestConnection();

// Configuração do middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeHome API v1");
});

app.UseHttpsRedirection();

// Ativando o CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
