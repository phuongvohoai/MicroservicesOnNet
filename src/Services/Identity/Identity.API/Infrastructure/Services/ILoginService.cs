namespace Identity.API.Infrastructure.Services
{
    using System.Threading.Tasks;

    public interface ILoginService<T>
    {
        Task<bool> ValidateCredentials(T user, string password);

        Task<string> LoginAsync(string userName, string password);
    }
}
