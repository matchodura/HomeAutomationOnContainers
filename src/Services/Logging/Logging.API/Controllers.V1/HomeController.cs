using Entities;
using Microsoft.AspNetCore.Mvc;
using Logging.API.Utilities;
using Logging.API.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Logging.API.Services.MQTT;
using System.Threading;
using Logging.API.DTOs;

namespace Logging.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMqttClientService _mqttClientService;


        public HomeController(ILogger logger, IUnitOfWork unitOfWork, MqttClientServiceProvider provider)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mqttClientService = provider.MqttClientService;
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
                pythonResult = await SciptRunner.RunPythonScript(scriptPath);

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


        //TODO searching on Logging via bash command
        [Route("mijia/search")]
        [HttpPost]
        public async Task<ActionResult<string>> FindAvailableBLESensors()
        {

            _logger.Information("Searching for nearest BLE devices...");

            string bashResult = string.Empty;
            string scriptPath = @"/home/pi/ble_scan.sh";


            try
            {
                _logger.Information($"Running bash script at path: {scriptPath}");
                bashResult = await SciptRunner.RunBashScript(scriptPath);

            }
            catch (Exception ex)
            {
                _logger.Error("An error occurred. {ErrorMessage} - {StackTrace}", ex.Message, ex.StackTrace);
                return BadRequest("Error occured!");
            }

            if (string.IsNullOrEmpty(bashResult))
            {
                _logger.Error("Failed obtaining results from running bash script!");

                return BadRequest("Failed getting bash results!");
            }          
            return Ok(bashResult);
        }

        [HttpGet]
        [Route("MQTT")]
        public async Task<ActionResult> GetValuesFromSensor()
        {
            string topic = "cmnd/czujnik/status";
            string payload = "10";

            _logger.Information("dupa");

            string subscribeTopic = "stat/czujnik/STATUS10";

            await _mqttClientService.SetupTopic(subscribeTopic);
            await _mqttClientService.PublishMessage(topic, payload);    
            var response = _mqttClientService.GetResponse();

            if (!string.IsNullOrEmpty(response))
            {
                DHT22 dHT22 = JsonSerializer.Deserialize<DHT22>(response);

                return Ok(dHT22);
            }
            return NoContent();
        }       
    }
}
