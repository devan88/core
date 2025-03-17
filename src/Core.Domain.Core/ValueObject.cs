namespace Core.Domain.Core
{
    /// <summary>
    /// Represents the base class for value objects.
    /// Value objects are immutable and are compared based on their property values.
    /// They are typically used to represent descriptive aspects of the domain with no conceptual identity.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// When overridden in a derived class, provides the properties used to determine equality of the value object.
        /// </summary>
        /// <returns>An enumerable collection of objects used in equality comparisons.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) => HashCode.Combine(current, obj));
        }
    }
}
