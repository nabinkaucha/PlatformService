using AutoMapper;
using CommandsService.DTOs;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // <source, target>
            CreateMap<Platform, PlatformReadDTO>();
            CreateMap<CommandCreateDTO, Command>();
            CreateMap<Command, CommandReadDTO>();
            CreateMap<PlatformPublishedDTO, Platform>()
                .ForMember(target => target.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(target => target.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(target => target.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(target => target.Commands, opt => opt.Ignore());
        }
    }
}
