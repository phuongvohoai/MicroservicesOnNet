namespace Identity.Infrastructure.Repository.Interfaces
{
    using System.Threading.Tasks;
    using Entities;

    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : EntityBase;

        void Commit();

        Task CommitAsync();

        void RejectChanges();

        void Dispose();
    }
}
