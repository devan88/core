namespace Core.Domain.Core
{
    public interface IAggregateRepository<T, TId> where T : AggregateRoot<TId>
    {
        Task<TId> AddAggregateAsync(T aggregate, CancellationToken cancellationToken);
    }
}
