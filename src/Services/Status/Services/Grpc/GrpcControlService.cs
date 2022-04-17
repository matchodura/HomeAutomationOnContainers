using Grpc.Core;
using Status.API.Entities;
using Status.API.Services.MQTT;
using System.Text.Json;

namespace Status.API.Services.Grpc
{
    public class GrpcControlService : GrpcItemControl.GrpcItemControlBase
    {
        private readonly IMqttClientService _mqttClientService;
        private readonly MongoDataContext _deviceService;

        public GrpcControlService(MqttClientServiceProvider provider, MongoDataContext deviceService)
        {
            _deviceService = deviceService;
            _mqttClientService = provider.MqttClientService;

        }

        public override async Task<ControlSwitchResponse> ControlSwitch(ControlSwitchRequest request, ServerCallContext context)
        {
            var response = new ControlSwitchResponse();

            var topic = "cmnd/" + request.Topic + "/power";
            var payload = request.Command;
            await _mqttClientService.PublishMessage(topic, payload);

            //Wait 5 seconds so the client can update the gotten response
            Thread.Sleep(1500);
            string responseFromMqtt = _mqttClientService.GetResponse();
            _mqttClientService.CleanResponse();


            if (!string.IsNullOrEmpty(responseFromMqtt))
            {
                var serializedResponse = JsonSerializer.Deserialize<State>(responseFromMqtt);
                
                response.Response = serializedResponse?.POWER;

            }

            return await Task.FromResult(response);
        }
    }
}
