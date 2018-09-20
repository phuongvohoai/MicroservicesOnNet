namespace WAL.UserActivityLogs.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
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
        public async Task<ActionResult<IEnumerable<ActivityLogViewModel>>> GetByUserId(int id)
        {
            try
            {
                // Get activity logs by user id
                var activityLogs = this.activityLogService.GetByUserId(id);
                var activityLogViewModels = Mapper.Map<IList<ActivityLogViewModel>>(activityLogs);

                // TODO: FIX BY ME
                var identityEndpoint = this.configuration.GetValue<string>("IdentityApi");
                var usersApiUri = $"{identityEndpoint}/api/users/{id}";

                // Get user name by id from Identity service
                using (var client = new HttpClient())
                {
                    var httpRequestMsg = new HttpRequestMessage(HttpMethod.Get, usersApiUri);

                    var response = await client.SendAsync(httpRequestMsg);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserViewModel>(responseBody);
                }

                return Ok(activityLogViewModels);
            }
            catch (Exception ex)
            {
                throw;
            }
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
