using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;

namespace CommandsService.Endpoints
{
    public static class PlatformsAPI
    {
        public static void UsePlatformAPIs(this WebApplication app)
        {
            RouteGroupBuilder apiGroup = app.MapGroup("/api/commands/Platforms");

            apiGroup.MapPost("test", (Platform platform) =>
            {
                Console.WriteLine("--> Request Processed");
                return TypedResults.Ok("test success");
            });

            apiGroup.MapGet("/", (ICommandRepo _repo, IMapper _mapper) =>
            {
                Console.WriteLine("--> Getting all Platforms");
                return TypedResults.Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(_repo.GetAllPlatforms()));
            });
        }
    }
}
