using AspNetCoreRateLimit;
using CroHoliCityAPI.Data;
using CroHoliCityAPI.Repository;
using CroHoliCityAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*****************************LOGING*********************************/
builder.Services.AddSerilog(options =>
{
    options.MinimumLevel.Debug();
    options.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    options.WriteTo.Debug();
    options.WriteTo.Console();
});

/******************************SERVICES********************************/
builder.Services.AddTransient<IlokacijeRepo, LokacijeRepo>();
builder.Services.AddTransient<IKalendarRepo, KalendarRepo>();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Configuration.AddEnvironmentVariables();
//Debug.Print("env string: "+builder.Configuration["DATABASE_URL"]);
/******************************DATA********************************/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string url = builder.Configuration["DATABASE_URL"];
    // log mysql connection string
    Log.Information("url1: " + builder.Configuration["DATABASE_URL"]);
    string conString=BuildConnectionStringFromUrl(url);
    options.UseNpgsql(conString);
});

Log.Information("url1: " + builder.Configuration["DATABASE_URL"]);
/******************************VERSIONING********************************/
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

/******************************SWAGGER CONFIG********************************/

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "CroHoliCityAPI", Version = "v1", Description = "API Dohvaca sve lokacije u RH i neradne dane u RH" });
    //options.SwaggerDoc("v2", new OpenApiInfo { Title = "CroHoliCityAPI", Version = "v2" });
});

var app = builder.Build();


app.UseSwagger();

//https://github.com/swagger-api/swagger-ui/issues/3832
app.UseSwaggerUI(config =>
{
    config.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
    {
        ["activated"] = false
    };
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "CroHoliCityAPI v1");
    //config.SwaggerEndpoint("/swagger/v2/swagger.json", "CroHoliCityAPI v2");
});


//if (app.Environment.IsDevelopment()) {

//}

app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseAuthorization();

app.MapControllers();

//DbManager.InsertLokacijeDb(app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>());
//DbManager.InsertNeradniDani(app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>());

app.Run();


// Run this  update the database:
static string BuildConnectionStringFromUrl(string databaseUrl)
{
    var uri = new Uri(databaseUrl);
    string host = uri.Host;
    int port = uri.Port;
    string database = uri.AbsolutePath.TrimStart('/');
    string username = uri.UserInfo.Split(':')[0];
    string password = uri.UserInfo.Split(':')[1];

    string connectionString = $"Server={host};Port={port};Database={database};User Id={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

    return connectionString;
}
