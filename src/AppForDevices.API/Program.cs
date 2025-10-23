using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Data.Common;
using Microsoft.Data.Sqlite;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Diagnostics.Metrics;
using Microsoft.Data.SqlClient;
using AppForDevices.API.Data;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers()
//show definitions of enums as strings
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Add service for managing a sqlserver database that will be managed using ApplicationDBContext
// the connection to the database was defined in appsettings
string? connection2Database = Environment.GetEnvironmentVariable("DBConnection2Use");
// If we are using the Production Environment, then the AZURE DB should be used,
// otherwise the localdb or SQLite should be used
//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?source=recommendations&view=aspnetcore-7.0
switch (connection2Database)
{
    /*case "SQLite"://origin
      case "SQLite"://mio // espero que no me de problemas
        DbConnection _connection = new SqliteConnection("Filename=:memory:");//origin

    
        //DbConnection _connection = new SqliteConnection("Filename=:memory:");//mio
        //connection in case a persistent database is required
        //DbConnection _connection = new SqliteConnection("Data Source=Application.db;Cache=Shared");
        _connection.Open();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(_connection));
        break; */
    case "AzureSQL":

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("AzureSQL")));
        ////ALTERNATIVE WITHOUT USERSECRET: you must include:
        ////Password:"ThePasswordYouHaveSetinAZURE";
        ////in the environmentvariable AZURESQL
        //ALTERNATIVE WITH USERSCRET
        //read the connectionstring to the AzureSQL db from launchsetting
        //    var conStrBuilder = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("AzureSQL"));

        //    conStrBuilder.Password = builder.Configuration["DbPassword"];
        //    var dbConnection = new SqlConnection(conStrBuilder.ConnectionString);
        //    builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        //                   opt.UseSqlServer(dbConnection));
        break;
    default:

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        break;
}
//Add Identity services to the container
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1",
    new OpenApiInfo
    {
        Title = "AppForMovies.API",
        Version = "v1",
        Description = "This API provides services for renting and purchasing movies",
        License = new OpenApiLicense { Name = "MIT License", Url = new Uri("https://opensource.org/license/mit/") },
        Contact = new OpenApiContact { Name = "Software Engineering II Team", Email = "isii@on.uclm.es" },
    });
    
    options.CustomOperationIds(apiDescription =>
    {
        return apiDescription.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
    options.EnableAnnotations(true, true);

    //options.EnableAnnotations(true, true);
});
var app = builder.Build();
//Map Identity routes
//app.MapIdentityApi<IdentityUser>();
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //it creates the DB in case it does not exist
        //this is used only while developing the system
        if (connection2Database == "SQLite")
            db.Database.EnsureCreated();
        else
            db.Database.Migrate();
        //it sees the database
        SeedData.Initialize(db, scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        
        c.DisplayOperationId();
    });
}
app.UseHttpsRedirection();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
app.UseAuthorization();
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
app.MapControllers();
app.Run();
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}