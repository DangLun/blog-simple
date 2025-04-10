using Asp.Versioning;
using Query.API.DependencyInjection.Extensions;
using Query.API.DependencyInjection.Options;
using Query.API.Middleware;
using Query.Application.DependencyInjection.Extension;
using Query.Presentation.Abstractions;
using Query.Persistence.DependencyInjection.Extensions;
using Query.Presentation.Common;
using System.Reflection;
using Query.Infrastructure.Authentication;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Query.Infrastructure.DependencyInjection.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
var serviceName = "Blog API Query";

//config cors
var clientHost = builder.Configuration.GetSection("CORSConfig")["ClientHost"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder
                .WithOrigins(clientHost!)
                .AllowAnyHeader()
                .AllowAnyMethod().AllowCredentials();
        });
});

//register controllers
builder.Services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly());
//register api configuration
builder.Services.AddSingleton(new ApiConfig { Name = serviceName });
//Configure swagger
builder.Services.ConfigureOptions<SwaggerConfigureOptions>();

builder.Services.AddInfrastructureRegisterServices();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Configure JWT authentication
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
var keyBytes = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

//Configure api versioning
builder.Services.AddApiVersioning(
        options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new QueryStringApiVersionReader());
        })
    .AddMvc()
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseApiLayerSwagger();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseExceptionHandler();
app.UseAuthorization();
app.MapPresentation();
app.Run();