using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeControl.API.Controllers
{
    public class SensorController : BaseApiController
    {
        private readonly ILogger _logger;

        public SensorController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.Information("Hello from Sensor Controller!");
            return Ok();
        }
    }
}
