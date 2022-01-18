using Microsoft.AspNetCore.Mvc;
using RaspberryPI.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPI.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("rpi/v{version:apiVersion}/[controller]")]
    public class HomeController : Controller
    {

        [Route("mijia")]
        [HttpGet]
        public IActionResult GetValuesFromBLE()
        {
   
            var scriptPath = @"/home/pi/mijia/MiTemperature2/LYWSD03MMC.py";

            Console.WriteLine(scriptPath);

            var result = PythonRunner.RunScript(scriptPath);

            //_logger.Information("Hello from xiaomi ble Controller!");
            return Ok(result);
        }
    }
}
