using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Query.Presentation.DependencyInjection.Extensions
{
    /// <summary>
    /// Extension methods for adding services to the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The service collection with added services</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Add AutoMapper with the executing assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}