namespace Identity.API.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Entity;

    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        Task<ApplicationUser> FindByUserNameAsync(string userName);
    }
}
