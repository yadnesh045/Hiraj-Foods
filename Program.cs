using Hiraj_Foods.Repository.IRepository;
using Hiraj_Foods.Repository;
using Microsoft.EntityFrameworkCore;
using Hiraj_Foods.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Hiraj_Foods.Models.View_Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Disable Caching

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseCacheAttribute
    {
        Location = ResponseCacheLocation.None,
        NoStore = true
    });
});



builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // You can set Time 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Admin/Login";
       });


builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
var key = builder.Configuration.GetValue<string>("StripeSettings:SecretKey");


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            NoCache = true,
            NoStore = true,
            MustRevalidate = true
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Pragma] = new string[] { "no-cache" };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Expires] = new string[] { "0" };
    await next();
});





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Yadnesh}/{action=Home}/{id?}");

app.Run();
