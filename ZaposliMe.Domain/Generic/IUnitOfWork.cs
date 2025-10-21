using ZaposliMe.Domain.Interfaces;

namespace ZaposliMe.Domain.Generic
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class; 
        IJobRepository Jobs { get; }            
        //IEmployeeReviewRepository EmployeeReviewRepositories { get; }            
        IEmployerReviewRepository EmployerReviews { get; }            

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
