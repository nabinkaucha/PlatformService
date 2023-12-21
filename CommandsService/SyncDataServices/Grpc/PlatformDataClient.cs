using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using PlatformService;
using static PlatformService.GrpcPlatform;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling Grpc Service {_configuration["GrpcPlatform"]}");
            GrpcChannel channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            GrpcPlatformClient client = new(channel);
            GetAllRequest request = new();

            try
            {
                PlatformResponse reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect Grpc Server {ex.Message}");
                return null;
            };
        }
    }
}
