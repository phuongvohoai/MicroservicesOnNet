using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Repositories;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private ApplicationUserContext context;

        public AccountsController(ApplicationUserContext c)
        {
            this.context = c;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return this.context.ApplicationUsers.Select(u => u.UserName).ToArray();
        }
    }
}