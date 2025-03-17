using Core.Domain.Core.Extensions;

namespace Core.Domain.Core
{
    /// <summary>
    /// Default implementation of IDomainEventDispatcher
    /// </summary>
    public sealed class DefaultDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public async Task DispatchAsync(DomainEvent domainEvent, CancellationToken cancellationToken)
        {
            IEnumerable<Task> tasks = _serviceProvider.GetDomainEventHandlers(domainEvent, [cancellationToken]);

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
    }
}
