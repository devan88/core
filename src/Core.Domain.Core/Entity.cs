namespace Core.Domain.Core
{
    /// <summary>
    /// Represents the base class for all entities in the domain with a generic identifier.
    /// An entity is defined by its identity rather than its attributes.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier (e.g., Guid, int, etc.).</typeparam>
    public abstract class Entity<TId>
    {
        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public TId Id { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
        /// </summary>
        protected Entity(TId id)
        {
            Id = id;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            // If both are transient (default value) then they are not considered equal.
            if (IsTransient() && other.IsTransient())
                return false;

            return Id!.Equals(other.Id);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return IsTransient() ? default(TId)!.GetHashCode() : Id!.GetHashCode();
        }

        /// <summary>
        /// Determines whether the current entity is transient (i.e., has not been assigned a valid identifier).
        /// </summary>
        /// <returns>
        /// true if the entity's identifier equals the default value for TId; otherwise, false.
        /// </returns>
        private bool IsTransient()
        {
            return EqualityComparer<TId>.Default.Equals(Id, default);
        }
    }
}
