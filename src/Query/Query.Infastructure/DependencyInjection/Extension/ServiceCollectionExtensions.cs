using Query.Domain.Abstractions.Auth;
using Query.Infrastructure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Query.Infrastructure.DependencyInjection.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureRegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IJwtProvider, JwtProvider>();
            return services;
        }
    }
}
