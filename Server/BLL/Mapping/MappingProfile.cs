using AutoMapper;
using global::Project.models;
using global::Project.models.DTOs;
using Project.models;
using Project.models.DTOs;

namespace Project.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Gift, GiftDto>()
                // .ForMember(dest => dest.DonorName, opt => opt.MapFrom(src => src.Donor.FullName))
                .ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<LoginDto, User>();
        }
    }
}
