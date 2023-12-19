using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(
            IPlatformRepo repo,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            Console.WriteLine("--> Getting all platforms");

            IEnumerable<Platform> platformsFromDb = await _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformsFromDb));
        }

        [HttpGet("{id:int}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            Console.WriteLine(string.Format("--> Getting platform with id {0}", id));

            Platform? platformFromDb = await _repo.GetPlatformById(id);

            if (platformFromDb == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PlatformReadDto>(platformFromDb));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platform)
        {
            Platform platformToSave = _mapper.Map<Platform>(platform);

            _repo.CreatePlatform(platformToSave);
            _repo.SaveChanges();

            Console.WriteLine(string.Format("--> Platform saved with id {0}", platformToSave.Id));

            PlatformReadDto savedPlatform = _mapper.Map<PlatformReadDto>(platformToSave);

            //send sync message
            // try
            // {
            //     await _commandDataClient.SendPlatformToCommand(platform);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"--> Could not send synchronously. {ex.Message}");
            // }

            //send async message
            try
            {
                PlatformPublishDto publishDto = _mapper.Map<PlatformPublishDto>(savedPlatform);
                publishDto.Event = "Platform_Published";
                _messageBusClient.PublishNewPlatform(publishDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send Asynchronously. {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = savedPlatform.Id }, savedPlatform);
        }

    }
}