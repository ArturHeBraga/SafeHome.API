using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SafeHome;
using SafeHome.Data;
using SafeHome.Models;
using SafeHome.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configurar MongoDB e Identity
var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

builder.Services.AddIdentityMongoDbProvider<ApplicationUser, ApplicationRole>(identityOptions =>
{
    identityOptions.Password.RequiredLength = 6;
    identityOptions.Password.RequireLowercase = false;
    identityOptions.Password.RequireUppercase = false;
    identityOptions.Password.RequireNonAlphanumeric = false;
    identityOptions.Password.RequireDigit = false;
},
mongoIdentityOptions =>
{
    mongoIdentityOptions.ConnectionString = mongoDbSettings.ConnectionString;
    mongoIdentityOptions.UsersCollection = builder.Configuration["IdentitySettings:UserCollection"];
    mongoIdentityOptions.RolesCollection = builder.Configuration["IdentitySettings:RoleCollection"];
    mongoIdentityOptions.DatabaseName = mongoDbSettings.DatabaseName;
});

// Add Singleton for MongoDB Service
builder.Services.AddSingleton<MongoDbService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // Adicionar autenticação
app.UseAuthorization();

app.MapControllers();

app.Run();
