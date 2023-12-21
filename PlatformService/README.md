# Platform Service

Created with `dotnet new webapi -n PlatformService -f net7.0`.
When new platform is created, publishes it to RabbitMQ message bus.

## Endpoints
### GET /api/platforms 
Get all platforms

### GET /api/platforms/{platformId}
Get platform with given id. Return 404 if not found.

### POST /api/platforms
```
{
    Name: string,
    Publisher: string,
    Cost: string
}
```
Create new platform