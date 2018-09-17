namespace Identity.Infrastructure.Repository.Interfaces
{
    using Microsoft.EntityFrameworkCore;

    public interface IDataContextFactory
    {
        DbContext Create();
    }
}
