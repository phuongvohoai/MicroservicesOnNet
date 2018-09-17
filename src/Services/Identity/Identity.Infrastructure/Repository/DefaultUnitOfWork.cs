namespace Identity.Infrastructure.Repository
{
    using Interfaces;

    public class DefaultUnitOfWork : UnitOfWorkBase
    {
        public DefaultUnitOfWork(IDataContextFactory dataContextFactory) : base(dataContextFactory.Create())
        {
        }

        public override IRepository<T> GetRepository<T>()
        {
            return new GenericRepository<T>(this.dataContext);
        }
    }
}
