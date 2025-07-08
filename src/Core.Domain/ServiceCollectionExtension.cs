using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain
{
    /// <summary>
    /// Provides extension methods for registering domain event components with the DI container.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Registers the default domain event dispatcher.
        /// </summary>
        /// <param name="services">The service collection to register the dispatcher with.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with the dispatcher registration.</returns>
        public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services)
        {
            return services.AddSingleton<IDomainEventDispatcher, DefaultDomainEventDispatcher>();
        }

        /// <summary>
        /// Registers all domain event handlers found in assemblies that satisfy the given predicate.
        /// </summary>
        /// <param name="services">The service collection for DI registrations.</param>
        /// <param name="predicate">
        /// An optional function that takes an <see cref="Assembly"/> and returns a Boolean value indicating
        /// whether the assembly should be scanned for domain event handlers. Defaults to all assembly.
        /// </param>
        /// <returns>The updated <see cref="IServiceCollection"/> with domain event handler registrations.</returns>
        public static IServiceCollection RegisterDomainEventHandlers(
            this IServiceCollection services,
            Func<Assembly, bool>? predicate = null)
        {
            services.Scan(scan => scan
               .FromApplicationDependencies(predicate ?? (assembly => true))
               .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)))
               .AsImplementedInterfaces()
               .WithTransientLifetime());

            return services;
        }
    }
}
