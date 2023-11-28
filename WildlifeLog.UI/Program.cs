using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI;
using WildlifeLog.UI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Inject HttpClient
builder.Services.AddHttpClient();


//Inject Session Configuration 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    // Configure session options as needed
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//////////////////////////////////////////////////////////
//Inject dbOCntext 
builder.Services.AddDbContext<WildlifeLogDbContext>();

//inject identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<WildlifeLogDbContext>()
	.AddDefaultTokenProviders();

//inject cloudinary 
builder.Services.AddScoped<IImageRepository, CloudinaryRepository>();

////////////////////////////////////////////////////////////
///
// Configure authentication cookie options
builder.Services.ConfigureApplicationCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set your desired expiration time
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.SlidingExpiration = true;
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
