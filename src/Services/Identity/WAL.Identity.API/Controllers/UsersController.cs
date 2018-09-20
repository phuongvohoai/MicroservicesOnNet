namespace WAL.Identity.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using AutoMapper;
    using Entities;
    using EventBus.Abstract;
    using Helpers;
    using IntegrationEvents;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Services;
    using ViewModels;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly AppSettings appSettings;
        private readonly IPubSub pubSub;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IPubSub pubSub)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.appSettings = appSettings.Value;
            this.pubSub = pubSub;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<UserViewModel> Authenticate([FromBody]UserViewModel userViewModel)
        {
            var user = userService.Authenticate(userViewModel.Username, userViewModel.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // TODO: Add more information
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7), // TODO: Get from configuration file
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            this.pubSub.PublishAsync(new ActivityLogAddingEvent()
            {
                UserId = user.Id,
                Activity = "Authenticate",
                Detail = $"Authenticate user {user.Id} successfully",
                LogDate = DateTime.UtcNow.ToLongDateString()
            });

            // return basic user info (without password) and token to store client side
            return Ok(new UserViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody]UserViewModel userViewModel)
        {
            // map view model to entity
            var user = mapper.Map<User>(userViewModel);

            try
            {
                // save 
                userService.Create(user, userViewModel.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserViewModel>> GetAll()
        {
            var users = userService.GetAll();
            var userViewModels = mapper.Map<IList<UserViewModel>>(users);
            return Ok(userViewModels);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult<UserViewModel> GetById(int id)
        {
            var user = userService.GetById(id);
            var userViewModel = mapper.Map<UserViewModel>(user);
            return Ok(userViewModel);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody]UserViewModel userViewModel)
        {
            // map view model to entity and set id
            var user = mapper.Map<User>(userViewModel);
            user.Id = id;

            try
            {
                // save 
                userService.Update(user, userViewModel.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            userService.Delete(id);
            return Ok();
        }
    }
}
