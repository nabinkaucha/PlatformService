using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            PlatformResponse response = new();
            Task<IEnumerable<Platform>> res = _repo.GetAllPlatforms();
            IEnumerable<Platform> platforms = res.Result;

            // response.Platform = platforms.Select(_mapper.Map<GrpcPlatformModel>);

            foreach (Platform plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}