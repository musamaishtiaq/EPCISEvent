using EPCISEvent.Interfaces;
using EPCISEvent.MasterData;
using EPCISEvent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MasterDataContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<MasterDataSeeder>();

string fastntBaseUrl = builder.Configuration.GetValue<string>("FastntBaseUrl");
builder.Services
    .AddRefitClient<IFastntApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(fastntBaseUrl));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//await SeedDatabaseAsync(app);
app.Run();


async Task SeedDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<MasterDataContext>();
        var seeder = services.GetRequiredService<MasterDataSeeder>();

        // Apply any pending migrations
        await context.Database.MigrateAsync();

        // Seed initial data
        await seeder.SeedAsync();

        Console.WriteLine("Database seeded successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}