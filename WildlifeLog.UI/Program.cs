using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI;
using WildlifeLog.UI.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Inject HttpClient
builder.Services.AddHttpClient();

//inject logging 
builder.Services.AddLogging(builder =>
{
	builder.AddConsole(); // Add other logging providers if needed
});

builder.Services.AddControllersWithViews();

//Inject Session Configuration 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	// Configure session options as needed
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.Name = "SessionCookie";
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

//Inject dbOCntext 
builder.Services.AddDbContext<WildlifeLogDbContext>();
builder.Services.AddDbContext<WildlifeLogAuthDbContext>();

//inject identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<WildlifeLogAuthDbContext>()
	.AddDefaultTokenProviders();

//inject cloudinary 
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();


// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set your desired expiration time
	options.Cookie.Name = "AuthCookie";
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.SlidingExpiration = true;
	options.LoginPath = "/Auths/Login";
	options.Cookie.Path = "/Auths/Login"; 
	options.AccessDeniedPath = "/Auths/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.SameSite = SameSiteMode.None;


});




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


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
