using Entities;
using Entities.BLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPI.API.Interfaces
{
    public interface IMijiaRepository
    {
        void AddValuesForMijia(Mijia mijia);
        Task<IEnumerable<Mijia>> GetAllValuesForMijia(string sensorName);

        void AddDevice(Device device);
    }
}
