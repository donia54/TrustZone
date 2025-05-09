using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
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
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Services.Chat;
using TrustZoneAPI.Hubs;
using System.Security.Claims;
using TrustZoneAPI.Services.SignalR;
using TrustZoneAPI.Services.AccessibilityFeatures;
using TrustZoneAPI.Services.Events;

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


builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IConversationService, ConversationService>();

builder.Services.AddScoped<ITMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();



builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IPlaceFeatureRepository, PlaceFeatureRepository>();
builder.Services.AddScoped<IPlaceFeatureRepository, PlaceFeatureRepository>();
builder.Services.AddScoped<IPlaceFeatureService, PlaceFeatureService>();

builder.Services.AddScoped<IFavoritePlaceRepository, FavoritePlaceRepository>();
builder.Services.AddScoped<IFavoritePlaceService, FavoritePlaceService>();

builder.Services.AddScoped<IDisabilityTypeService, DisabilityTypeService>();
builder.Services.AddScoped<IDisabilityTypeRepository, DisabilityTypeRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Services.AddScoped<IAccessibilityFeatureRepository, AccessibilityFeatureRepository>();
builder.Services.AddScoped<IAccessibilityFeatureService, AccessibilityFeatureService>();

builder.Services.AddScoped<IBranchPhotoRepository, BranchPhotoRepository>();
builder.Services.AddScoped<IBranchPhotoService, BranchPhotoService>();


builder.Services.AddScoped<ISignalRMessageSender, SignalRMessageSender>();

builder.Services.AddSingleton<IConnectionService, ConnectionService>();


builder.Services.AddScoped<chatHub>();

builder.Services.AddSignalR();

// For CurrenUserId
builder.Services.AddHttpContextAccessor();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

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


        NameClaimType = ClaimTypes.NameIdentifier
    };


    o.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub")) 
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500",
                "http://localhost:5500",
                "https://localhost:5500",
                "https://trustzone.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod()
        .AllowCredentials();
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy
//            .AllowAnyOrigin()
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});


builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();


var app = builder.Build();


app.UseRouting();


app.UseCors();

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

app.MapHub<chatHub>("/chatHub");

app.Run();
