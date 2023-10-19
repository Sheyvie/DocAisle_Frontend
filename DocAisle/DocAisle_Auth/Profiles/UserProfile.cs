using DocAisle_Auth.Model;

namespace DocAisle_Auth.Profiles
{
    public class UserProfile:AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationResponse, ApplicationUser>().ReverseMap()
            .ForMember(dest => dest.UserName, u => u.MapFrom(reg => reg.Email));

            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}
