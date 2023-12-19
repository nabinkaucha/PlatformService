using AutoMapper;
using CommandsService.Data;
using CommandsService.DTOs;
using CommandsService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Endpoints
{
    public static class CommandsAPI
    {
        public static void UseCommandsAPIs(this WebApplication app)
        {
            RouteGroupBuilder apiGroup = app.MapGroup("/api/commands/Platforms/{platformId}/Commands");

            apiGroup.MapGet("/", GetCommandsForPlatform);

            apiGroup.MapGet("/{commandId}", GetCommandForPlatform).WithName("GetCommandForPlatform");

            apiGroup.MapPost("/", CreateCommandForPlatform);
        }

        private static IResult GetCommandsForPlatform(int platformId, ICommandRepo _repo, IMapper _mapper)
        {
            Console.WriteLine($"--> Getting commands associated with platform id {platformId}");
            if (_repo.PlatformExists(platformId))
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(_repo.GetCommandsForPlatform(platformId)));
        }

        private static IResult GetCommandForPlatform(int platformId, int commandId, ICommandRepo _repo, IMapper _mapper)
        {
            Console.WriteLine($"--> Getting command associated with platform id {platformId} with command id {commandId}");
            if (_repo.PlatformExists(platformId))
            {
                return TypedResults.NotFound();
            }
            Command? command = _repo.GetCommand(platformId, commandId);
            if (command == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(_mapper.Map<CommandReadDTO>(command));
        }

        private static IResult CreateCommandForPlatform(int platformId, [FromBody] CommandCreateDTO createCommand, ICommandRepo _repo, IMapper _mapper)
        {
            Console.WriteLine($"--> Creating command for platform {platformId} with command {createCommand.CommandLine}");
            if (!_repo.PlatformExists(platformId))
            {
                return TypedResults.NotFound();
            }
            try
            {
                Command command = _mapper.Map<Command>(createCommand);
                _repo.CreateCommand(platformId, command);
                if (_repo.SaveChanges())
                {
                    return TypedResults.CreatedAtRoute(value: _mapper.Map<CommandReadDTO>(command),
                                                       routeName: "GetCommandForPlatform",
                                                       routeValues: new { platformId, commandId = command.Id }
                                                      );
                }
                return TypedResults.BadRequest();
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(ex.Message);
            }

        }
    }
}
