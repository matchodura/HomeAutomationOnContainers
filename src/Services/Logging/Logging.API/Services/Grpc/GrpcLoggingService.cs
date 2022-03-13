using AutoMapper;
using Grpc.Core;
using Logging.API.Interfaces;
using System.Threading.Tasks;

namespace Logging.API.Services.Grpc
{
    public class GrpcLoggingService : GrpcLogging.GrpcLoggingBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GrpcLoggingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<LoggingApiResponse> GetAllLoggingValues(GetAllRequest request, ServerCallContext context)
        {
            var response = new LoggingApiResponse();
            var dhts = await _unitOfWork.SensorRepository.GetAllValues();

            foreach (var dht in dhts)
            {
                response.Dht.Add(_mapper.Map<GrpcDHTModel>(dht));
            }

            return await Task.FromResult(response);
        }

        public override async Task<LoggingApiSensorValueResponse> GetSensorLoggingValue(GetSensorValue request, ServerCallContext context)
        {
            var response = new LoggingApiSensorValueResponse();
            var value = await _unitOfWork.SensorRepository.GetLastValueForDht(request.SensorTopic);
                        
            var valueToSend = _mapper.Map<GrpcSensorModel>(value);

            response.Sensor = valueToSend;

            return await Task.FromResult(response);
        }
    }
}
