using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using ShopIndexWebApp.Server;
using ShopIndexWebApp.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContextFactory<DataContext>(o =>
{
    o.UseSqlite(connectionString);
    //o.UseLazyLoadingProxies();
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseNotFoundImageMiddleware();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

var dbFactory = app.Services.GetRequiredService<IDbContextFactory<DataContext>>();
using var db = await dbFactory.CreateDbContextAsync();
await db.Database.MigrateAsync();
await db.DisposeAsync();

app.Run();
