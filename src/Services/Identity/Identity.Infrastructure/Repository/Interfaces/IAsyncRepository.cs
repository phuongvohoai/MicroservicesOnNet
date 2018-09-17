namespace Identity.Infrastructure.Repository.Interfaces
{
    using System.Threading.Tasks;
    using Entities;

    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<T> AddAsync(T entity);

        Task<T> GetByIdAsync(int id);

        Task RemoveAsync(T entity);

        Task UpdateAsync(T entity);
    }
}
