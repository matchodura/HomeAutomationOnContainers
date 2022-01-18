using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorLogging.API.Data
{
    public class Constants
    {
        //TODO, RPI.API client when all endpoints are done
        public const string RPI_IP_ADDRESS = "http://192.168.1.181/api/v1/home/mijia";
        public const string MIJIA_SCRIPT_PATH = @"/home/pi/mijia/MiTemperature2/script.py";
        public const string CLI_DELIMITER = " ";
    }
}
