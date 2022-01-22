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
            var dhts =  await _unitOfWork.DHTRepository.GetAllValues();

            foreach (var dht in dhts)
            {
                response.Dht.Add(_mapper.Map<GrpcDHTModel>(dht));
            }

            return await Task.FromResult(response);
        }
    }
}
