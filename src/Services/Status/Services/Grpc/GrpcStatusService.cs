using AutoMapper;
using Grpc.Core;
using Status.API.DTOs;
using System.Threading.Tasks;

namespace Status.API.Services.Grpc
{
    public class GrpcStatusService : GrpcItem.GrpcItemBase
    {
        private readonly IMapper _mapper;
        private readonly MongoDataContext _deviceService;
        public GrpcStatusService(IMapper mapper, MongoDataContext deviceService )
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        public override async Task<StatusApiResponse> GetItemFromStatus(GetItemRequest request, ServerCallContext context)
        {
            var response = new StatusApiResponse();
            //var dhts = await _unitOfWork.DHTRepository.GetAllValues();

            //foreach (var dht in dhts)
            //{
            //    response.Dht.Add(_mapper.Map<GrpcDHTModel>(dht));
            //}
            var test = await _deviceService.GetAsync(request.DeviceName);

            var test2 = _mapper.Map<GrpcItemModel>(test);

            response.Item = test2;
            return await Task.FromResult(response);
        }

        public override async Task<StatusApiAllItemsResponse> GetAllItemsFromStatus(GetAllItemsRequest request, ServerCallContext context)
        {
            var response = new StatusApiAllItemsResponse();
            //var dhts = await _unitOfWork.DHTRepository.GetAllValues();

            //foreach (var dht in dhts)
            //{
            //    response.Dht.Add(_mapper.Map<GrpcDHTModel>(dht));
            //}

            var test = await _deviceService.GetAsync();
            return await Task.FromResult(response);
        }
    }
}
