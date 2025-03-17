using Microsoft.Extensions.DependencyInjection;

namespace Core.Domain.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the IServiceProvider to retrieve domain event handlers.
    /// </summary>
    public static class ServiceProviderExtension
    {
        /// <summary>
        /// Retrieves all registered domain event handlers for the specified event type, 
        /// dynamically invokes their HandleAsync method with the given event and additional parameters,
        /// and returns an enumerable of Tasks representing the asynchronous operations.
        /// </summary>
        /// <typeparam name="TEvent">The type of the domain event.</typeparam>
        /// <param name="provider">The IServiceProvider to resolve the event handlers from.</param>
        /// <param name="domainEvent">The domain event instance to be handled.</param>
        /// <param name="parameters">Any additional parameters to pass to the handler method.</param>
        /// <returns>An enumerable collection of Tasks for each invoked event handler.</returns>
        public static IEnumerable<Task> GetDomainEventHandlers<TEvent>(
            this IServiceProvider provider,
            TEvent domainEvent,
            params object[] parameters)
            where TEvent : DomainEvent
        {
            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());

            IEnumerable<object?> handlers = provider.GetServices(handlerType);

            foreach (object? handler in handlers)
            {
                // Dynamically invoke the Handle method on the handler.
                System.Reflection.MethodInfo? method = handlerType
                    .GetMethod(nameof(IDomainEventHandler<TEvent>.HandleAsync));

                if (method != null)
                {
                    object? result = method.Invoke(handler, [domainEvent, .. parameters]);
                    if (result is Task task)
                    {
                        yield return task;
                    }
                }
            }
        }
    }
}
