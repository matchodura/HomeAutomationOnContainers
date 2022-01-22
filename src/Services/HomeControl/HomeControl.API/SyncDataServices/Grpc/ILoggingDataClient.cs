using Entities.DHT;
using System.Collections.Generic;

namespace HomeControl.API.SyncDataServices.Grpc
{
    public interface ILoggingDataClient
    {
        List<DHT> ReturnAllDhts();
    }
}
