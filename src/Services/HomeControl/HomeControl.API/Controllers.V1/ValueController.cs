using AutoMapper;
using HomeControl.API.Controllers.v1;
using HomeControl.API.DTOs.LoggingAPI;
using HomeControl.API.Interfaces;
using HomeControl.API.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace HomeControl.API.Controllers.V1
{
    public class ValueController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGrpcClient _grpcClient;

        public ValueController(ILogger logger, IUnitOfWork unitOfWork, IMapper mapper, IGrpcClient grpcClient)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _grpcClient = grpcClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        [Route("test")]
        [HttpGet]
        public async Task<ActionResult<SensorValueDTO>> GetLastValueByName(string topic)
        {
            var item = _grpcClient.ReturnLastSensorValue(topic);

            return Ok(item);
        }
    }
}
