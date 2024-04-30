namespace Claims.Infrastructure.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        // Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default, bool trackable = true);
        // void Add(T entity);
        // void Add(IEnumerable<T> entries);
        // void Update(T entity);
        // IUnitOfWork UnitOfWork { get; }
        
        Task<T> CreateAsync(T item, CancellationToken cancellationToken = default);
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
