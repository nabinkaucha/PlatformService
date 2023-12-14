using CommandsService.Models;

namespace CommandsService.Endpoints
{
    public static class PlatformsAPI
    {
        public static void UsePlatformAPIs(this WebApplication app)
        {
            RouteGroupBuilder apiGroup = app.MapGroup("/api/Platforms");

            apiGroup.MapPost("test", (Platform platform) =>
            {
                return TypedResults.Ok("test success");
            });
        }
    }
}
