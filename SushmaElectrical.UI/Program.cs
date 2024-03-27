
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SushmaElectrical.Repositories;
using SushmaElectrical.Repositories.Implementations;
using SushmaElectrical.Repositories.Interfaces;
using SushmaElectrical.UI.DDL;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SushmaElectrical.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LogoutPath = "/Identity/Account/Logout";
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("SEConnectionStringDB"),
b => b.MigrationsAssembly("SushmaElectrical.UI")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICategoryRepo,CategoryRepo>();
builder.Services.AddScoped<IProductRepo,ProductRepo>();
builder.Services.AddScoped<DDLHelpers>();
builder.Services.AddScoped<IUtilityRepo,UtilityRepo>();
builder.Services.AddScoped<IEmailSender,EmailSender>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICartRepo,CartRepo>();
builder.Services.AddScoped<IDbInitial,DbInitial>();
builder.Services.AddScoped<IOrderRepo,OrderRepo>();
builder.Services.AddScoped<IShippingRepo,ShippingRepo>();
builder.Services.AddScoped<IPaymentModeRepo,PaymentModeRepo>();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

DataSeeding();
void DataSeeding()
{
    using(var scope = app.Services.CreateScope())
    {
        var _dbRepo = scope.ServiceProvider.GetRequiredService<IDbInitial>();
        _dbRepo.Seed();
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
