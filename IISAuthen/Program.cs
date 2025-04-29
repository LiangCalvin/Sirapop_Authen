using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IISAuthen.Services;
using IISAuthen.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Read Jwt Key
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null.");

// 2. Read CORS Origins
var corsOrigins = builder.Configuration.GetValue<string>("Cors") ?? "";
var myAllowSpecificOrigins = "_myAllowSpecificOrigins"; // name of the CORS policy

// 3. Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(corsOrigins.Split(",", StringSplitOptions.RemoveEmptyEntries))
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials(); // if your frontend needs to send cookies or auth headers
        });
});

// 4. Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
          {
              // Use logging instead of Console.WriteLine
              var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
              var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
              logger.LogInformation($"Received token: {token?.Substring(0, Math.Min(token?.Length ?? 0, 20))}...");
              return Task.CompletedTask;
          },
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError($"Authentication failed: {context.Exception.Message}");
            if (context.Exception.InnerException != null)
            {
                logger.LogError($"Inner exception: {context.Exception.InnerException.Message}");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Token validated successfully");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning($"Challenge issued: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});

// 5. Add Authorization
builder.Services.AddAuthorization();

// 6. Add Controllers
builder.Services.AddControllers();

// 7. Add your service
builder.Services.AddScoped<IAuthenService, AuthService>();

// 8. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IISAuthen API",
        Version = "v1",
        Description = "API for authentication and authorization"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "<b>How to authenticate:</b><br/>" +
            "1. Get a token from /api/Auth/login<br/>" +
            "2. Enter the token in the format: <b>Bearer {your_token}</b><br/>" +
            "<b>Example:</b> Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
var app = builder.Build();

// 9. Middleware
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

// 10. Map Controllers (you missed this)
app.MapControllers();

app.Run();