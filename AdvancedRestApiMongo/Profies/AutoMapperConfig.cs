using AdvancedRestApiMongo.DTOs;
using AdvancedRestApiMongo.Models;
using AutoMapper;

namespace AdvancedRestApiMongo.Profies
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
