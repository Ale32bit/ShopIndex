using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ShopIndex.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContextFactory<DataContext>(o =>
{
    o.UseSqlite(connectionString);
    o.UseLazyLoadingProxies();
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllers();

var dbFactory = app.Services.GetRequiredService<IDbContextFactory<DataContext>>();
using var db = await dbFactory.CreateDbContextAsync();
await db.Database.MigrateAsync();
await db.DisposeAsync();

app.Run();
