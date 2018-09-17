using Identity.API.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Identity.API.Infrastructure.Services
{
    using Entity;

    public class LoginService : ILoginService<ApplicationUser>
    {
        private readonly IApplicationUserRepository userRepository;

        public LoginService(IApplicationUserRepository repository)
        {
            this.userRepository = repository;
        }

        public Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
