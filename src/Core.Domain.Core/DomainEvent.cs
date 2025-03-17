namespace Core.Domain.Core
{
    /// <summary>
    /// Serves as the base class for all domain events.
    /// A domain event is used to capture a significant occurrence in the domain.
    /// </summary>
    public abstract record DomainEvent
    {
        /// <summary>
        /// Gets the date and time when the event occurred.
        /// </summary>
        public DateTimeOffset CreatedOn { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEvent"/> class.
        /// Sets the OccurredOn property to the current UTC time.
        /// </summary>
        protected DomainEvent()
        {
            CreatedOn = DateTime.UtcNow;
        }
    }
}
