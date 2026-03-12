using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ETSU_Marketplace.Hubs;
using ETSU_Marketplace.Services;
using ETSU_Marketplace.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlite(
      builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options => 
    {
        options.SignIn.RequireConfirmedAccount = false;
        // Configure other options as needed
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IItemListingRepository, DbItemListingRepository>();
builder.Services.AddScoped<ILeaseListingRepository, DbLeaseListingRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IUserRepository, DbUserRepository>();
builder.Services.AddSignalR();


var app = builder.Build();

app.UseHttpMethodOverride();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // This replaces 'dotnet ef database update'
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Add Razor Pages for Identity UI
app.MapRazorPages();

app.MapHub<MarketplaceHub>("/marketplaceHub");


app.Run();
