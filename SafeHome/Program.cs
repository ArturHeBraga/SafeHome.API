using SafeHome;
using SafeHome.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SafeHome API", Version = "v1" });
});
builder.Services.AddSingleton<MongoDbService>();

var app = builder.Build();

var mongoDbService = app.Services.GetRequiredService<MongoDbService>();
mongoDbService.TestConnection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeHome API v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
