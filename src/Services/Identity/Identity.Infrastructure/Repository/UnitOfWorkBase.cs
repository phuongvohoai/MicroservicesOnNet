namespace Identity.Infrastructure.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        #region Variables

        protected readonly DbContext dataContext;

        #endregion

        #region Implement IUnitOfWork

        protected UnitOfWorkBase(DbContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public abstract IRepository<T> GetRepository<T>() where T : EntityBase;

        public void Commit()
        {
            this.dataContext.SaveChanges();
        }

        public Task CommitAsync()
        {
            return this.dataContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.dataContext.Dispose();
        }

        public void RejectChanges()
        {
            var entries = this.dataContext.ChangeTracker.Entries();
            var changeTrackers = entries.Where(e => e.State != EntityState.Unchanged);
            foreach (var entry in changeTrackers)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        #endregion
    }
}
