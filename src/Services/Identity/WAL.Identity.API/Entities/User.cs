using Microsoft.AspNetCore.Identity;

namespace WAL.Identity.API.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}