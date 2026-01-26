using Microsoft.Extensions.DependencyInjection;

namespace Core.HttpClient.Authorization
{
    public static class KeyedServiceCollectionExtensions
    {
        public static KeyedDecoratorBuilder<TService> AddTransient<TService, TImplementation>(
        this IServiceCollection services,
        object key)
        where TService : class
        where TImplementation : class, TService
        {
            services.AddKeyedTransient<TService, TImplementation>(key);
            return new KeyedDecoratorBuilder<TService>(services, key, typeof(TImplementation));
        }

        /// <summary>
        /// Decorates a registered service of type <typeparamref name="TService"/>
        /// with a decorator of type <typeparamref name="TDecorator"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to be decorated.</typeparam>
        /// <typeparam name="TDecorator">The type of the decorator that implements <typeparamref name="TService"/>.
        /// </typeparam>
        /// <param name="services">The service collection to which the decoration will be applied.</param>
        /// <param name="key">A unique key to identify the service to be decorated.</param>
        /// <returns>The updated service collection.</returns>
        /// <exception cref="ArgumentException">Thrown when no registered service of type
        /// <typeparamref name="TService"/> with the specified key is found.
        /// </exception>
        public static IServiceCollection Decorate<TService, TDecorator>(
            this IServiceCollection services,
            object key)
            where TDecorator : TService
        {
            ServiceDescriptor? descriptor = services
                .FirstOrDefault(sd => sd.ServiceKey?.Equals(key) == true && sd.ServiceType.Equals(typeof(TService)))
                ?? throw new ArgumentException($"Could not find any registered services for type '{typeof(TService)}' with key '{key}'.");

            string innerKey = $"__inner_{key}_{Guid.NewGuid():N}";

            services.Remove(descriptor);
            services.RegisterDecoratedService<TService>(descriptor, innerKey);
            services.RegisterDecorator<TService, TDecorator>(descriptor, innerKey, key);
            return services;
        }

        private static IServiceCollection RegisterDecorator<TService, TDecorator>(
            this IServiceCollection services,
            ServiceDescriptor descriptor,
            string innerKey,
            object decoratorKey)
        {
            ObjectFactory decoratorFactory = ActivatorUtilities.CreateFactory(typeof(TDecorator), [typeof(TService),]);
            ServiceDescriptor decoratorDescriptor = new(typeof(TService), decoratorKey, (sp, key) =>
            {
                TService innerService = (TService)sp.GetRequiredKeyedService(typeof(TService), innerKey);
                return (TDecorator)decoratorFactory(sp, [innerService]);
            }, descriptor.Lifetime);
            services.Add(decoratorDescriptor);

            return services;
        }

        private static IServiceCollection RegisterDecoratedService<TService>(
            this IServiceCollection services,
            ServiceDescriptor descriptor,
            string innerKey)
        {
            if (descriptor.KeyedImplementationFactory is not null)
            {
                ServiceDescriptor innerDescriptor = new(
                    typeof(TService),
                    innerKey,
                    descriptor.KeyedImplementationFactory,
                    descriptor.Lifetime);

                services.Add(innerDescriptor);
            }
            if (descriptor.KeyedImplementationType is not null)
            {
                ServiceDescriptor innerDescriptor = new(
                    typeof(TService),
                    innerKey,
                    descriptor.KeyedImplementationType,
                    descriptor.Lifetime);

                services.Add(innerDescriptor);
            }

            return services;
        }
    }
}
