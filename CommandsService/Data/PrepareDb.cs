using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepareDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            IPlatformDataClient grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            IEnumerable<Platform> platforms = grpcClient.ReturnAllPlatforms();

            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }

        private static void SeedData(ICommandRepo _repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new Platforms...");
            foreach (Platform platform in platforms)
            {
                if (!_repo.ExternalPlatformExists(platform.ExternalId))
                {
                    _repo.CreatePlatform(platform);
                }
                _repo.SaveChanges();
            }
        }
    }
}
