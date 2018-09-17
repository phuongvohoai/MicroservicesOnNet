namespace Identity.Infrastructure.Repository
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<T> : IRepository<T> where T : EntityBase
    {
        #region Variables

        private readonly DbContext dbContext;

        private DbSet<T> dbSet => this.dbContext.Set<T>();

        #endregion

        #region Default constructor

        public GenericRepository(DbContext dataContext)
        {
            this.dbContext = dataContext;
            this.dbContext.Database.EnsureCreated();
        }

        #endregion

        #region Implement IRepository

        public IQueryable<T> Entities => this.dbSet;

        public T Add(T entity)
        {
            return this.dbSet.Add(entity).Entity;
        }

        public void Remove(T entity)
        {
            this.dbSet.Remove(entity);
        }

        public T GetById(int id)
        {
            return this.dbSet.Find(id);
        }

        public void Update(T entity)
        {
            var dbEntityEntry = this.dbContext.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
        }

        #endregion

        #region Implement IAsyncRepository

        public Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
