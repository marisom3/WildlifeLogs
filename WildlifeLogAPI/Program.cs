using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Mappings;
using WildlifeLogAPI.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//AddSerilog 
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "WildlifeLog API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name= JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },

            new List<string>()
        }
    });

});


//Inject DbContext 
builder.Services.AddDbContext<WildlifeLogDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WildlifeLogConnectionString")));

//Inject AuthDbCOntext
builder.Services.AddDbContext<WildlifeLogAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("WildlifeLogAuthConnectionString")));

//Inject ParkRepository
builder.Services.AddScoped<IParkRepository, ParkRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

//Inject CategoryRepository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//Inject IToken Repository 
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

//Inject IImageRepository 
builder.Services.AddScoped<IImageRepository, ImageRepository>();

//Inject Automapper 
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

//Inject Identity 
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("WildlifeLog")
    .AddEntityFrameworkStores<WildlifeLogAuthDbContext>()
    .AddDefaultTokenProviders();



//Add Identity Options (password requirements?)
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});


//Install Authentification 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();

app.UseAuthorization();

//reroute the localhosturl to the images folder inside of the api so users ccan access the image 
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
    //https//localhost:1234/Images now when we go to this url it will redirect us tot eh images folde rinside the api 
});


app.MapControllers();

app.Run();
