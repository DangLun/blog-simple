using Asp.Versioning;
using Command.API.DependencyInjection.Extensions;
using Command.API.DependencyInjection.Options;
using Command.API.Middleware;
using Command.Application.DependencyInjection.Extension;
using Command.Infrastructure.Authentication;
using Command.Infrastructure.Configurations;
using Command.Infrastructure.DependencyInjection.Extension;
using Command.Infrastructure.EmailServices;
using Command.Persistence.DependencyInjection.Extensions;
using Command.Presentation.Abstractions;
using Command.Presentation.Common;
using Contract.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
var serviceName = "Blog API Command";

// config cors
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

// register jwtProvider
builder.Services.Configure<GoogleAuthSettings>(builder.Configuration.GetSection("GoogleAuth"));
builder.Services.AddInfrastructureRegisterServices();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddHttpContextAccessor();

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

// static file path
string uploadDir = Path.Combine(Const.GetSolutionDir(), Const.UPLOAD_DIRECTORY);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadDir),
    RequestPath = $"/{Const.REQUEST_PATH_STATIC_FILE}"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseApiLayerSwagger();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapPresentation();
app.Run();