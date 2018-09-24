namespace WAL.UserActivityLogs.API.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoMapper;
    using Entities;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using ViewModel;

    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService activityLogService;
        private readonly IConfiguration configuration;

        public ActivityLogsController(IActivityLogService activityLogService, IConfiguration configuration)
        {
            this.activityLogService = activityLogService;
            this.configuration = configuration;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
            //var activityLogs = this.activityLogService.GetAll();
            //var activityLogViewModels = Mapper.Map<IList<ActivityLogViewModel>>(activityLogs);
            //return Ok(activityLogViewModels);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ActivityLogViewModel>>> GetByUserId(int userId)
        {
            // Get activity logs by user id
            var activityLogs = this.activityLogService.GetByUserId(userId);
            var activityLogViewModels = Mapper.Map<IList<ActivityLogViewModel>>(activityLogs);

            // TODO: FIX BY ME
            var identityEndpoint = this.configuration.GetValue<string>("IdentityApi");
            var usersApiUri = $"{identityEndpoint}/api/users";

            // Get user name by id from Identity service
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync(usersApiUri);
            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserViewModel>(responseBody);

            return Ok(activityLogViewModels);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ActivityLogViewModel log)
        {
            var activityLog = Mapper.Map<ActivityLog>(log);
            this.activityLogService.Create(activityLog);
        }
    }
}
