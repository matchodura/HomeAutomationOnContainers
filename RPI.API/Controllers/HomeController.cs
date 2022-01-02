using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RaspberryPI.API.Utilities;
using System;

namespace SensorLogging.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : Controller
    {
        //private readonly ILogger _logger;

        //public HomeController(ILogger logger)
        //{
        //    _logger = logger;
        //}

        [HttpGet]
        public IActionResult Index()
        {
           // _logger.LogInformation("Hello from swagger!");
            return new RedirectResult("~/swagger");
        }

        [Route("mijia")]
        [HttpPost]
        public IActionResult GetValuesFromBLE([FromBody] string scriptPath)
        {

            //_logger.LogInformation("Hello from rpi mijia runner!");
            Console.WriteLine("Hello from rpi mijia runner!");

            try
            {
                //todo: add mac adress and retries number
                PythonRunner.RunScript(scriptPath);

                return Ok("Script completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erorr occured: {ex.Message}");
            }

            return NotFound("Nie dziala!");
        }
    }
}
