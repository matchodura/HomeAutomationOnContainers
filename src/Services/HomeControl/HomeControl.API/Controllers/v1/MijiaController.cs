using Entities;
using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HomeControl.API.Controllers.v1
{
    public class MijiaController : BaseApiController
    {

        private readonly IHttpClientFactory _clientFactory;

        private readonly ILogger _logger;

        public MijiaController(IHttpClientFactory clientFactory, ILogger logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }
                     
        [HttpGet]
        public async Task<ActionResult<Mijia>> GetValuesFromBLE([FromQuery] string macAddress,
            [FromQuery] string sensorName)
        {

            //maybe change to stringbuilder in the future for more customization?
            string scriptPath = Constants.MIJIA_SCRIPT_PATH +
                    Constants.CLI_DELIMITER +
                    macAddress +
                    Constants.CLI_DELIMITER +
                    sensorName;
                       
            var httpClient = _clientFactory.CreateClient();
            var content = new StringContent(
                    JsonSerializer.Serialize(scriptPath),
                    Encoding.UTF8,
                    "application/json");

            using var httpResponse = await httpClient.PostAsync(Constants.RPI_IP_ADDRESS, content);


            httpResponse.EnsureSuccessStatusCode();

            var contents = await httpResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(contents))
            {
                _logger.Error("Response did not contain valid values!");

                return BadRequest("Something went wrong");
            }

            var result = JsonSerializer.Deserialize<Mijia>(contents);                                  

            //PushProperty in place of ForContext to make it a global prop
            _logger.ForContext("Temperature", result.Temperature)
                .ForContext("Humidity", result.Humidity)
                .ForContext("Voltage", result.Voltage)
                .ForContext("Battery", result.Battery)
                .ForContext("Sensor", result.SensorName)
                .ForContext("MacAddress", result.MacAddress)
                .ForContext("TimestampUTC", result.TimestampLinux)
                .Information("Successfully obtained values for mijia sensor!");

            return Ok(result);
        }

        [HttpGet]
        [Route("values")]
        public async Task<ActionResult<IEnumerable<Mijia>>> GetValuesForMijia([FromQuery] string sensorName)
        {

           // string endpoint = @$"/values?sensorName={macAddress}";

            var httpClient = _clientFactory.CreateClient();
            var content = new StringContent(
                    JsonSerializer.Serialize(sensorName),
                    Encoding.UTF8,
                    "application/json");

            using var httpResponse = await httpClient.GetAsync(Constants.RPI_IP_ADDRESS + $"/values?sensorName={sensorName}");


            httpResponse.EnsureSuccessStatusCode();

            var contents = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<IEnumerable<Mijia>>(contents);

            return Ok(result);
        }
    }
}
