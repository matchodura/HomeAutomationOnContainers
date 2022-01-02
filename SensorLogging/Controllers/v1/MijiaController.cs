using Microsoft.AspNetCore.Mvc;
using RaspberryPI.API.Utilities;
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

        public MijiaController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
                      
        [HttpGet]
        public async Task<IActionResult> GetValuesFromBLE([FromQuery] string url, [FromQuery] string scriptPath)
        {

            var httpClient = _clientFactory.CreateClient();
            var content = new StringContent(
                    JsonSerializer.Serialize(scriptPath),
                    Encoding.UTF8,
                    "application/json");

            using var httpResponse = await httpClient.PostAsync(url, content);

            httpResponse.EnsureSuccessStatusCode();

            var contents = await httpResponse.Content.ReadAsStringAsync();

            return Ok($"RPI said: {contents}");
        }



        /// <summary>
        /// Endpoint on which the RPI publishes gathered results
        /// </summary>
        /// <param name="name"></param>
        /// <param name="temp"></param>
        /// <param name="hum"></param>
        /// <param name="bat"></param>
        /// <returns></returns>
        [Route("values")]
        [HttpGet]
        public IActionResult GatherResults([FromQuery] string name, [FromQuery] string temp,
                 [FromQuery] string hum, [FromQuery] string bat)
        {
            string[] test = new string[4];
            test[0] = name;
            test[1] = temp;
            test[2] = hum;
            test[3] = bat;

            return Ok(test);
        }

    }
}
