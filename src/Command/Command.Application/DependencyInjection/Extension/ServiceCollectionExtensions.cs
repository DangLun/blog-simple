using System.Reflection;
using Command.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Command.Application.DependencyInjection.Extension
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register application services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register mediator
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
            // Register validation pipeline
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // Register fluent validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}