using Contract.Abstractions.Services;
using Command.Domain.Abstractions.Auth;
using Command.Infrastructure.Authentication;
using Command.Infrastructure.EmailServices;
using Microsoft.Extensions.DependencyInjection;
using Command.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Command.Application.Abstractions;
using Command.Infrastructure.Services;

namespace Command.Infrastructure.DependencyInjection.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureRegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGoogleAuthSettings>(
                    x => x.GetRequiredService<IOptions<GoogleAuthSettings>>().Value);

            services.AddScoped<IFileService, FileService>();
            return services;
        }
    }
}
