using Microsoft.AspNetCore.Mvc;
using SensorLogging.API.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorLogging.API.Controllers.v1
{


    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class XiaomiBLEController : Controller
    {
        private readonly ILogger _logger;

        public XiaomiBLEController(ILogger logger)
        {
            _logger = logger;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    _logger.Information("Hello from Sensor Controller!");
        //    return Ok();
        //}

        [Route("mijia")]
        [HttpGet]
        public IActionResult GetValuesFromBLE([FromQuery] string macAdress,
            [FromQuery] int retries,
            [FromQuery] string scriptPath)
        {
            //var scriptPath = @"C:\skrypt.py";
            //var scriptPath = @"/home/pi/mijia/MiTemperature2/LYWSD03MMC.py";
            var scriptParams = "-d " + macAdress + " -r -b -c " + retries;

            Console.WriteLine(scriptPath);
            Console.WriteLine(scriptParams);
            var result = "test";

            string pythonVersion = "python3";

            //var result = PythonRunner.RunScript(pythonVersion, scriptPath, scriptParams);
            _logger.Information("Hello from xiaomi ble Controller!");
            return Ok(result);
        }

        [Route("mijia/test")]
        [HttpGet]
        public IActionResult GetValuesFromBLE()
        {
            var mainLocation = @"/home/pi/";
            var scriptPath = mainLocation + @"mijia/MiTemperature2/";
            var dataLocation = mainLocation + @"HomeAutomation/";

            string[] scriptParams = new string[10];

            scriptParams[0] = "-d";
            scriptParams[1] = "A4:C1:38:48:31:DF";
            scriptParams[2] = "-r";
            scriptParams[3] = "-b";
            scriptParams[4] = "-c";
            scriptParams[5] = "1";
            scriptParams[6] = "--name";
            scriptParams[7] = "MySensor";
            scriptParams[8] = "--callback";
            scriptParams[9] = "sendToFile.sh";

            string pythonVersion = "python3";

            Console.WriteLine(scriptPath);
            Console.WriteLine(scriptParams);

            var result = PythonRunner.RunScript(pythonVersion, scriptPath, dataLocation, scriptParams);
            _logger.Information("Hello from xiaomi ble Controller!");
            return Ok(result);
        }

    }
}
