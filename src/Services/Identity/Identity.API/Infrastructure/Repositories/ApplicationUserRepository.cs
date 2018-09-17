namespace Identity.API.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Entity;

    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationUserContext context;

        public ApplicationUserRepository(ApplicationUserContext context)
        {
            this.context = context;
        }

        public Task<ApplicationUser> FindByUserNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
