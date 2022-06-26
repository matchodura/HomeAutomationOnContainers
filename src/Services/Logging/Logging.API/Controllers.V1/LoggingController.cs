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
using Entities.DHT;
using static Logging.API.Extensions.DateTimeExtensions;
using Logging.API.Filters;

namespace Logging.API.Controllers
{
    public class LoggingController : BaseApiController
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMqttClientService _mqttClientService;


        public LoggingController(ILogger logger, IUnitOfWork unitOfWork,  IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;    
        }


        [HttpGet]
        [Route("mqtt")]
        public async Task<ActionResult> GetValuesFromSensor()
        {
            string topic = "cmnd/pokoj/czujnik_1/status";
            string payload = "10";

            _logger.Information("dupa");

            string subscribeTopic = "stat/pokoj/czujnik_1/STATUS10";

            //await _mqttClientService.SetupSubscriptionTopic(subscribeTopic);
            await _mqttClientService.PublishMessage(topic, payload);    
            var response = _mqttClientService.GetResponse();

            //if (!string.IsNullOrEmpty(response))
            //{
            //    DHTDTO serializedResponse = JsonSerializer.Deserialize<DHTDTO>(response);

            //    var result = _mapper.Map<DHT>(serializedResponse);

            //    result.SensorName = "testowy";

            //    //if not used - failes see-> https://github.com/npgsql/efcore.pg/issues/2000
            //    var currentDate = DateTime.UtcNow;
            //    result.Time = currentDate;

            //    _unitOfWork.DHTRepository.AddValuesForDHT(result);

            //    if (await _unitOfWork.Complete()) return Ok(result);

            //}

            return NoContent();
        }

        [HttpGet]
        [Route("mqtt/values")]
        public async Task<ActionResult<IEnumerable<DHT>>> GetAllDHTValues([FromQuery] string sensorName)
        {
            var dhtValues = await _unitOfWork.SensorRepository.GetAllValuesForDht(sensorName);

            if (dhtValues.Count() == 0) return NotFound("Sensor with that name does not exist!");

            return Ok(dhtValues);
        }

        [HttpGet]
        [Route("mqtt/values/all")]
        public async Task<ActionResult<IEnumerable<DHT>>> GetAllSensorValues()
        {
            var dhtValues = await _unitOfWork.SensorRepository.GetAllValues();

            if (dhtValues.Count() == 0) return NotFound("Sensor with that name does not exist!");

            return Ok(dhtValues);
        }

        [HttpGet]
        [Route("mqtt/values/last")]
        public async Task<ActionResult<IEnumerable<DHT>>> GetLastDHTValue([FromQuery] string sensorName)
        {
            var dhtValue = await _unitOfWork.SensorRepository.GetLastValueForDht(sensorName);

            if (dhtValue == null) return NotFound("Sensor with that name does not exist!");

            return Ok(dhtValue);
        }

        [HttpGet]
        [Route("devices")]
        public async Task<ActionResult<IEnumerable<string>>> GetDevicesWithLoggedValues()
        {
            var devices = await _unitOfWork.SensorRepository.GetDevicesWithValuesInDatabase();

            if (devices.Count() == 0) return NotFound("No devices are currently configured!");

            return Ok(devices);
        }


        //new api commands for frontend
        [HttpGet]
        [Route("values")]
        public async Task<IActionResult> GetSensorValues([FromQuery] string sensorName, [FromQuery] DateFilter dateFilter)
        {
            Thread.Sleep(2000);
            var sensorValues = await _unitOfWork.SensorRepository.GetAllValuesForSensorWithTimeSpan(sensorName, dateFilter);

            if (sensorValues.Count() == 0) return NotFound("Sensor with that name does not exist!");

            var groupedBySensorName = sensorValues.GroupBy(x => new { x.SensorName, x.Topic}).Select(g => new
            {
                Name = g.Key.SensorName,
                Topic = g.Key.Topic,
                Values = g.Select(x => new { x.Temperature, x.Humidity, x.DewPoint, x.Time }).OrderBy(x => x.Time)
            });

            return Ok(groupedBySensorName.First());
        }
    }
}
