using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppForDevices.Web.Components;
using AppForDevices.Web.Components.Account;
using AppForDevices.Web.Data;
using AppForDevices.Web.API;
using AppForDevices.Web.Components.Pages.Purchases;
using AppForDevices.Web.Components.Pages.Reviews;
using AppForDevices.Web.Components.Pages.Rentals;
using AppForDevices.Web.Components.Pages.Repairs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

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
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddScoped<PurchaseStateContainer>();
builder.Services.AddScoped<RentalStateContainer>();
builder.Services.AddScoped<DeviceForReviewStateContainer>();
builder.Services.AddSingleton<ReceiptStateContainer>();
builder.Services.AddScoped<AppForDevicesAPIClient>(sp =>
        new AppForDevicesAPIClient(Environment.GetEnvironmentVariable("AppForDevices_API"), new HttpClient())
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
