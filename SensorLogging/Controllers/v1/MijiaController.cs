using Entities;
using Microsoft.AspNetCore.Mvc;
using SensorLogging.API.Data;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SensorLogging.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MijiaController : Controller
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

            //TODO: check if this makes sense
            if (contents is null)
            {
                _logger.Error("Response did not contain valid values!");

                return BadRequest("Something went wrong");
            }


            var result = JsonSerializer.Deserialize<Mijia>(contents);

            //TODO do it more cleanly, either save timestamp to database and then convert
            //or convert on the moment the result are comming and then save it to db
            //or ignore and let the frontend convert the result

            var time = UnixTimeStampToDateTime(result.Timestamp);

            //PushProperty in place of ForContext to make it a global prop
            _logger.ForContext("Temperature", result.Temperature)
                .ForContext("Humidity", result.Humidity)
                .ForContext("Voltage", result.Voltage)
                .ForContext("Battery", result.Battery)
                .ForContext("Sensor", result.SensorName)
                .ForContext("MacAddress", result.MacAddress)
                .ForContext("Timestamp", time)
                .Information("Successfully obtained values for mijia sensor!");

            return Ok(result);
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}
