using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Services;
using TrustZoneAPI.Services.Misc;
using TrustZoneAPI.Services.Users;
using TrustZoneAPI.Services.Categories;
using TrustZoneAPI.Services.Places;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using TrustZoneAPI.MiddleWares;
using TrustZoneAPI.Services.Azure;
using TrustZoneAPI.Services.Disabilities;

var builder = WebApplication.CreateBuilder(args);


// إضافة HttpClient إلى DI Container
builder.Services.AddHttpClient();

// إضافة خدمات إلى الحاوية
builder.Services.AddControllers();
builder.Services.AddScoped<CurrentUserIdActionFilter>();  // إضافة الفلتر
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<CurrentUserIdActionFilter>();  // إضافة الفلتر للمشروع بأكمله
});
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>( options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddScoped<IPlaceRepository, PlaceRepository>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IBranchOpeningHourRepository, BranchOpeningHourRepository>();
builder.Services.AddScoped<IBranchOpeningHourService, BranchOpeningHourService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserDisabilityRepository, UserDisabilityRepository>();
builder.Services.AddScoped<IUserDisabilityService, UserDisabilityService>();



builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddSignalR();


// For CurrenUserId
builder.Services.AddHttpContextAccessor();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false; 
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
    };
});



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   // app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapRazorPages();
app.MapHub<SearchHub>("/searchHub");
app.Run();
