using Entities;
using Microsoft.AspNetCore.Mvc;
using RaspberryPI.API.Utilities;
using RPI.API.Controllers;
using RPI.API.Extensions;
using RPI.API.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RPI.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
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

            if (string.IsNullOrEmpty(pythonResult))
            {
                _logger.Error("Failed obtaining results from running python script!");

                return BadRequest("Failed getting python results!");
            }


            var result = JsonSerializer.Deserialize<Mijia>(pythonResult);


            _unitOfWork.MijiaRepository.AddValuesForMijia(result);

            if (await _unitOfWork.Complete()) return Ok(result);

            return BadRequest("Failed to add entry!");
        }


        [HttpGet]
        [Route("mijia/values")]
        public async Task<ActionResult<IEnumerable<Mijia>>> GetAllValues([FromQuery] string sensorName)
        {
            var mijiaValues = await _unitOfWork.MijiaRepository.GetAllValuesForMijia(sensorName);

            if (mijiaValues.Count() == 0) return NotFound("Sensor with that name does not exist!");

            return Ok(mijiaValues);

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
