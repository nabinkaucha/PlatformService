using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool IsProduction)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), IsProduction);
        }

        private static void SeedData(AppDbContext? context, bool IsProduction)
        {
            if (context != null)
            {
                if (IsProduction)
                {
                    Console.WriteLine("--> Attemting migration...");
                    try
                    {
                        context.Database.Migrate();
                        Console.WriteLine("--> Migration successfull");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Migration failed. {ex.Message}");
                    }
                }
                if (!context.Platforms.Any())
                {
                    Console.WriteLine("--> Seeding data");
                    context.Platforms.AddRangeAsync(
                        new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                        new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                    );

                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("--> We already have data");
                }
            }

        }
    }
}