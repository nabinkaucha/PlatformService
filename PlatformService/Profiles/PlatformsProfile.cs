using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            // <source, target>
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(grpcPlat => grpcPlat.PlatformId, opt => opt.MapFrom(plat => plat.Id))
                .ForMember(grpcPlat => grpcPlat.Name, opt => opt.MapFrom(plat => plat.Name))
                .ForMember(grpcPlat => grpcPlat.Publisher, opt => opt.MapFrom(plat => plat.Publisher));
        }
    }
}