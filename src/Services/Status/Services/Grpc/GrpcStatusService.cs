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
        public GrpcStatusService(IMapper mapper, MongoDataContext deviceService)
        {
            _deviceService = deviceService;
            _mapper = mapper;
        }

        public override async Task<StatusApiResponse> GetItemFromStatus(GetItemRequest request, ServerCallContext context)
        {
            var response = new StatusApiResponse();
            var device = await _deviceService.GetAsync(request.DeviceName);             
            var deviceToSend = _mapper.Map<GrpcItemModel>(device);

            response.Item = deviceToSend;
            return await Task.FromResult(response);
        }

        public override async Task<StatusApiAllItemsResponse> GetAllItemsFromStatus(GetAllItemsRequest request, ServerCallContext context)
        {
            var response = new StatusApiAllItemsResponse();
            var devices = await _deviceService.GetAsync();

            foreach (var device in devices)
            {
                response.Item.Add(_mapper.Map<GrpcItemModel>(device));
            }

            return await Task.FromResult(response);
        }
    }
}
