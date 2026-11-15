using Ewallet.Mvc.Data;
using Ewallet.Mvc.Repositories;
using Ewallet.Mvc.Repository;
using Ewallet.Mvc.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("newEwallet") 
    ?? throw new InvalidOperationException("ga bisa koneksi, belum konfigurasi");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();

    app.UseExceptionHandler("/Home/Error");
    

    app.MapScalarApiReference("/api",options => 
    {
        options.Title = "Ewallet API";
    });
    
    app.MapGet("/", () => Results.Redirect("/api"));
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
