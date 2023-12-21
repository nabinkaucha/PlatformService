# Platform Service

Created with `dotnet new webapi -n CommandsService`.
Uses MinimialAPI.
Uses gRPC to get initial list of platforms form PlatformService.
Subscribes to RabbitMQ message bus for any platform created.

## Endpoints
### GET /api/commands/platforms
Get all platforms

### GET /api/commands/platforms/{platformId}/commands
Get commands available for given platformId. Returns 404 if not available.

### POST /api/commands/platforms/{platformId}/commands
```
{
    HowTo: string,
    CommandLine: string
}
```
Create new command for given platformId. Returns 404 if give platformId is not available.

### GET /api/commands/platforms/{platformId}/commands/{commandId}
Get single command for given platformId and commandID. Returns 404 if not available.