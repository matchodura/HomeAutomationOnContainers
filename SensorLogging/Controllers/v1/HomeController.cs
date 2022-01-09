using Microsoft.AspNetCore.Mvc;
using SensorLogging.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SensorLogging.API.Controllers
{
    public class HomeController : BaseApiController
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
        
        [HttpGet]
        [Route("seq/info")]
        public IActionResult Test(string dupa)
        {
            _logger.Information($"Test {dupa}");
            return Ok();
        }

        [HttpGet]
        [Route("seq/error")]
        public IActionResult TestError(string dupa)
        {          
            _logger.Error($"Test {dupa}");
            return Ok();
        }

        [HttpGet]
        [Route("seq/warn")]
        public IActionResult TestWarn(string dupa)
        {
            _logger.Warning($"Test {dupa}");
            return Ok();
        }

        [HttpGet]
        [Route("seq/exception")]
        public IActionResult TestException(string dupa)
        {
            try
            {
                // something that throws an exception, like...
                throw new Exception("This is my exception message");
            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred. {ErrorMessage} - {StackTrace}", ex.Message, ex.StackTrace);

            }

            return Ok();
        }
    }
}
