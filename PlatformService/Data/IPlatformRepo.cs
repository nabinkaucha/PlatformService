using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        bool SaveChanges();
        Task<IEnumerable<Platform>> GetAllPlatforms();
        Task<Platform?> GetPlatformById(int id);
        void CreatePlatform(Platform platform);
    }
}