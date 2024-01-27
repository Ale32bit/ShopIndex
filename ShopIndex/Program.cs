using Microsoft.EntityFrameworkCore;
using ShopIndex.Data;
using ShopIndex.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseSqlite(connectionString);
    //o.UseLazyLoadingProxies();
});

// Add services to the container.
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
app.UseNotFoundImageMiddleware();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<DataContext>();
await db.Database.EnsureCreatedAsync();
await db.DisposeAsync();
await scope.DisposeAsync();

app.Run();
