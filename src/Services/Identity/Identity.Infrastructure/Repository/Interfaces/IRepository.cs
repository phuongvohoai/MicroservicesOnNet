namespace Identity.Infrastructure.Repository.Interfaces
{
    using System.Linq;
    using Entities;

    public interface IRepository<T> : IAsyncRepository<T> where T : EntityBase
    {
        IQueryable<T> Entities { get; }

        T Add(T entity);

        T GetById(int id);

        void Remove(T entity);

        void Update(T entity);

    }
}
