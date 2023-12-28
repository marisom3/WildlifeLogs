using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI;
using WildlifeLog.UI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Inject HttpClient
builder.Services.AddHttpClient();

// inject logging 
builder.Services.AddLogging(builder =>
{
	builder.AddConsole(); // Add other logging providers if needed
});

builder.Services.AddControllersWithViews();

// Inject Session Configuration 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	// Configure session options as needed
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.Name = "SessionCookie";
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Inject dbContext 
builder.Services.AddDbContext<WildlifeLogDbContext>();
builder.Services.AddDbContext<WildlifeLogAuthDbContext>();

// inject identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<WildlifeLogAuthDbContext>()
	.AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;
});

// inject cloudinary 
builder.Services.AddScoped<IImageRepository, CloudinaryImageRepository>();

// Configure authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "AuthScheme";
    options.DefaultScheme = "AuthScheme";
	options.DefaultSignInScheme = "AuthScheme";
	options.DefaultChallengeScheme = "AuthScheme";
})
.AddCookie("AuthScheme", options =>
{
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set your desired expiration time
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.SlidingExpiration = true;
	options.AccessDeniedPath = "/Auths/AccessDenied";
	options.LoginPath = "/Auths/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
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

app.Run();