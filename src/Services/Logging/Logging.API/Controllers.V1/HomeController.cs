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
using AutoMapper;
using Entities.DHT22;
using static Logging.API.Extensions.DateTimeExtensions;

namespace Logging.API.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMqttClientService _mqttClientService;


        public HomeController(ILogger logger, IUnitOfWork unitOfWork, MqttClientServiceProvider provider, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        [Route("mqtt")]
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
                DHT22DTO serializedResponse = JsonSerializer.Deserialize<DHT22DTO>(response);

                var result = _mapper.Map<DHT22>(serializedResponse);

                result.SensorName = "testowy";

                //if not used - failes see-> https://github.com/npgsql/efcore.pg/issues/2000
                var currentDate = DateTime.UtcNow;
                result.Time = currentDate;

                _unitOfWork.DHTRepository.AddValuesForDHT(result);

                if (await _unitOfWork.Complete()) return Ok(result);

            }

            return NoContent();
        }

        [HttpGet]
        [Route("mqtt/values")]
        public async Task<ActionResult<IEnumerable<Mijia>>> GetAllDHTValues([FromQuery] string sensorName)
        {
            var dhtValues = await _unitOfWork.DHTRepository.GetAllValuesForDht(sensorName);

            if (dhtValues.Count() == 0) return NotFound("Sensor with that name does not exist!");

            return Ok(dhtValues);

        }
    }
}
