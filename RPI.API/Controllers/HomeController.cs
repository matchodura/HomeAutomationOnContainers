using Entities;
using Microsoft.AspNetCore.Mvc;
using RaspberryPI.API.Utilities;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SensorLogging.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        [Route("mijia")]
        [HttpPost]
        public async Task<ActionResult<Mijia>> GetValuesFromBLE([FromBody] string scriptPath)
        {

            string pythonResult = string.Empty;

            try
            {
                _logger.Information($"Running python script at path: {scriptPath}");
                pythonResult = await PythonRunner.RunScript(scriptPath);

            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred. {ErrorMessage} - {StackTrace}", ex.Message, ex.StackTrace);
                return BadRequest("Error occured!");
            }

            var result = JsonSerializer.Deserialize<Mijia>(pythonResult);

            return Ok(result);
        }

        //TODO searching on RPI via bash command
        [Route("mijia/search")]
        [HttpPost]
        public IActionResult FindAvailableBLESensors()
        {

            _logger.Information("Searching for nearest BLE devices...");

            return Ok();
        }

        //TODO getting more values RPI provide
    }
}
