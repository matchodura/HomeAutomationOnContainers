﻿using Microsoft.AspNetCore.Mvc;
using HomeControl.API.Controllers.v1;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MQTTClient;
using MQTTnet.Client;
using System.Text;

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

        [HttpGet]
        [Route("values")]
        public async Task<ActionResult> GetValuesFromSensor([FromQuery] string topicName)
        {
            string clientId = "TestClient";
            string clientUsername = "dotnet-app";
            string clientPassword = "dotnet-app";
            string brokerAddress = "192.168.1.181";
            int brokerPort = 1883;

            string[] topics = { "stat/czujnik/STATUS10", "stat/gniazdko/POWER" };



            var client = new Client(clientId, clientUsername, clientPassword, brokerAddress, brokerPort);

            await client.SetupClientConnection();
            await client.SubscribeToTopics(topics);

            var payload = String.Empty;

            client.MqttClient.UseApplicationMessageReceivedHandler(msg =>
            {
                payload = Encoding.UTF8.GetString(
                    msg?.ApplicationMessage?.Payload ?? Array.Empty<byte>());
            });


            return Ok(payload);
        }
    }
}