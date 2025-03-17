namespace Core.Domain.Core
{
    /// <summary>
    /// Represents an abstract repository for managing aggregates in a domain-driven design context.
    /// This repository provides methods for adding, retrieving, and updating aggregates,
    /// as well as handling domain events associated with those aggregates.
    /// </summary>
    /// <typeparam name="T">The type of the aggregate root.</typeparam>
    /// <typeparam name="TId">The type of the aggregate root's identifier.</typeparam>
    public abstract class AggregateRepository<T, TId> where T : AggregateRoot<TId>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        protected AggregateRepository(IDomainEventDispatcher domainEventDispatcher)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        /// <summary>
        /// Adds an aggregate asynchronously.
        /// </summary>
        public abstract Task<TId> AddAggregateAsync(T aggregate, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves an aggregate asynchronously by its identifier.
        /// </summary>
        public abstract Task<T> GetAggregateAsync(TId id, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an aggregate asynchronously.
        /// </summary>
        public abstract Task<T> UpdateAggregateAsync(T aggregate, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an aggregate asynchronously.
        /// </summary>
        public abstract Task<bool> DeleteAggregateAsync(TId id, CancellationToken cancellationToken);

        /// <summary>
        /// Runs domain events in parallel.
        /// </summary>
        protected Task RunDomainEventsInParallel(IReadOnlyCollection<DomainEvent>? domainEvents, CancellationToken cancellationToken)
        {
            if (domainEvents?.Count == 0)
            {
                return Task.CompletedTask;
            }

            List<Task> domainEventTasks = [];

            foreach (DomainEvent item in domainEvents!)
            {
                domainEventTasks.Add(_domainEventDispatcher.DispatchAsync(item, cancellationToken));
            }

            return Task.WhenAll(domainEventTasks);
        }

        /// <summary>
        /// Gets the tasks for dispatching domain events.
        /// </summary>
        protected IEnumerable<Task> GetDomainEventTasks(
            IReadOnlyCollection<DomainEvent>? domainEvents,
            CancellationToken cancellationToken)
        {
            if (domainEvents is null)
            {
                yield break;
            }

            foreach (DomainEvent item in domainEvents)
            {
                yield return _domainEventDispatcher.DispatchAsync(item, cancellationToken);
            }
        }
    }
}
