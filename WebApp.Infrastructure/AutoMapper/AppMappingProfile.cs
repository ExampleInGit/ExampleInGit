using AutoMapper;
using WebApp.Data.Entities.Classes;
using WebApp.Shared.Models;

namespace WebApp.Infrastructure.AutoMapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<CreateUserModel, User>();
        }
    }
}
