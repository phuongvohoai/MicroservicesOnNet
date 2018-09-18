namespace WAL.UserActivityLogs.API.Controllers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Entities;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using ViewModel;

    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService activityLogService;

        public ActivityLogsController(IActivityLogService activityLogService)
        {
            this.activityLogService = activityLogService;
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
        public ActionResult<IEnumerable<ActivityLogViewModel>> GetByUserId(int userId)
        {
            var activityLogs = this.activityLogService.GetByUserId(userId);
            var activityLogViewModels = Mapper.Map<IList<ActivityLogViewModel>>(activityLogs);
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
