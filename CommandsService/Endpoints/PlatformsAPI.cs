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
        }
    }
}
