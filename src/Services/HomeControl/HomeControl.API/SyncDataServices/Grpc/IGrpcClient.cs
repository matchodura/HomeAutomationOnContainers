using Entities.DHT;
using HomeControl.API.DTOs;
using System.Collections.Generic;

namespace HomeControl.API.SyncDataServices.Grpc
{
    public interface IGrpcClient
    {
        List<DHT> ReturnAllDhts();
        ItemDeviceDTO GetDeviceFromStatusAPI(string deviceName);
        List<ItemDeviceDTO> GetAllDevicesFromStatusAPI(string deviceName);
    }
}
