using Microsoft.Extensions.DependencyInjection;

namespace Core.HttpClient.Authorization
{
    public class KeyedDecoratorBuilder<TService>
    {
        private readonly IServiceCollection _services;
        private readonly object _key;
        private readonly Type _innerType;

        public IServiceCollection Services => _services;

        public KeyedDecoratorBuilder(IServiceCollection services, object key, Type innerType)
        {
            _services = services;
            _key = key;
            _innerType = innerType;
        }

        public KeyedDecoratorBuilder<TService> Decorate<TDecorator>()
            where TDecorator : class, TService
        {
            string innerKey = $"__inner_{_key}_{Guid.NewGuid():N}";

            // Move original to hidden inner key
            //var descriptor = _services.FirstOrDefault(d =>
            //    d.ServiceKeyEquals<TService>(_key) &&
            //    d.ImplementationType == _innerType &&
            //    d.Lifetime == ServiceLifetime.Transient);

            ServiceDescriptor descriptor = _services
                .FirstOrDefault(sd => sd.ServiceKey?.Equals(_key) == true && sd.ServiceType.Equals(typeof(TService)))
                ?? throw new ArgumentException($"Could not find any registered services for type '{typeof(TService)}' with key '{_key}'.");

            _services.Remove(descriptor);
            RegisterDecoratedService(descriptor, innerKey);
            RegisterDecoratorService<TDecorator>(descriptor, innerKey, _key);

            return this;
        }

        private void RegisterDecoratedService(
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

                _services.Add(innerDescriptor);
            }
            if (descriptor.KeyedImplementationType is not null)
            {
                ServiceDescriptor innerDescriptor = new(
                    typeof(TService),
                    innerKey,
                    descriptor.KeyedImplementationType,
                    descriptor.Lifetime);

                _services.Add(innerDescriptor);
            }
        }

        private void RegisterDecoratorService<TDecorator>(
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
            _services.Add(decoratorDescriptor);
        }
    }
}
