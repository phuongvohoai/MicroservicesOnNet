namespace WAL.Identity.API.Helpers
{
    using AutoMapper;
    using Entities;
    using ViewModels;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UserViewModel, User>();
        }
    }
}