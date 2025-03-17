using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Core
{
    /// <summary>
    /// Represents the base class for aggregate roots in the domain with a generic identifier.
    /// An aggregate root is an entity that serves as the entry point for an aggregate.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public abstract class AggregateRoot<TId> : Entity<TId>
    {
        private List<DomainEvent> _domainEvents;

        /// <summary>
        /// Gets a read-only collection of domain events that have been raised.
        /// </summary>
        public IReadOnlyCollection<DomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
        /// </summary>
        protected AggregateRoot(TId id)
            : base(id)
        {
            _domainEvents = [];
        }

        /// <summary>
        /// Adds a domain event to the collection of raised events.
        /// </summary>
        /// <param name="domainEvent">The domain event to add.</param>
        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents ??= [];
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        /// Clears all domain events in the aggregate.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
