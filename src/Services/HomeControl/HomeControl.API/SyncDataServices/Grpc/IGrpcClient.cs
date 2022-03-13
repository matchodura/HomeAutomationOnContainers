using Entities.DHT;
using HomeControl.API.DTOs;
using HomeControl.API.DTOs.LoggingAPI;
using System.Collections.Generic;

namespace HomeControl.API.SyncDataServices.Grpc
{
    public interface IGrpcClient
    {
        List<DHT> ReturnAllDhts();
        ItemDeviceDTO GetDeviceFromStatusAPI(string deviceName);
        List<ItemDeviceDTO> GetAllDevicesFromStatusAPI();
        SensorValueDTO ReturnLastSensorValue(string topic);
    }
}
