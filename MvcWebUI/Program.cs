using _038_ETradeCoreLite.Settings;
using DataAccess.Contexts;
using DataAccess.Services;
using DataAccess.Services.CRUD;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region Localization
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("en-US")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Accounts/Home/Login";
        config.AccessDeniedPath = "/Accounts/Home/AccessDenied";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        config.SlidingExpiration = true;
    });

builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddDbContext<Db>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddScoped<ProductServiceBase, ProductService>();
builder.Services.AddScoped<CategoryServiceBase, CategoryService>();
builder.Services.AddScoped<UserServiceBase, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<CountryServiceBase, CountryService>();
builder.Services.AddScoped<CityServiceBase, CityService>();
builder.Services.AddScoped<StoreServiceBase, StoreService>();
builder.Services.AddScoped<IReportService, ReportService>();

var section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});
#endregion

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

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Login}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
