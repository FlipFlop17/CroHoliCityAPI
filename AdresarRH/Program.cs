using AspNetCoreRateLimit;
using CroHoliCityAPI.Data;
using CroHoliCityAPI.Repository;
using CroHoliCityAPI.Repository.IRepository;
using CroHoliCityAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*****************************LOGING*********************************/
builder.Services.AddSerilog(options =>
{
    options.WriteTo.Console();
});

/******************************DATA********************************/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"));
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

