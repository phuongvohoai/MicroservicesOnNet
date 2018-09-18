namespace WAL.UserActivityLogs.API.Infrastructure
{
    using AutoMapper;
    using Entities;
    using ViewModel;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ActivityLog, ActivityLogViewModel>();
            CreateMap<ActivityLogViewModel, ActivityLog>();
        }
    }
}
