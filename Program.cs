using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RegistryRecord.Data;
using RegistryRecord.Repositories;
using RegistryRecord.Services;
using RegistryRecord.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==================== DbContext ====================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// ==================== Swagger ====================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RegistryRecord API",
        Version = "v1"
    });

    // ✅ JWT Ayarları
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT token girin. Örn: Bearer {token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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
    };

    c.AddSecurityRequirement(securityRequirement);
});

// ==================== JWT Ayarları ====================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

var keyBytes = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
var securityKey = new SymmetricSecurityKey(keyBytes);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = securityKey, 
            ClockSkew = TimeSpan.Zero
        };
    });

// ==================== Dependency Injection ====================
builder.Services.AddScoped<OfficerRepository>();
builder.Services.AddScoped<OfficerService>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<CreteToken>();
// Token helper
builder.Services.AddSingleton<CreteToken>();

// ==================== MVC & Swagger ====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==================== Middleware ====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();