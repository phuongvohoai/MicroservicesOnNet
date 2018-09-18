namespace WAL.Identity.API.ViewModels
{
    using Microsoft.AspNetCore.Mvc;

    public class UserViewModel : ActionResult
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}