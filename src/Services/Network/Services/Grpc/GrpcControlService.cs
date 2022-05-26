using Grpc.Core;
using Network.API.Entities;
using Network.API.Services.MQTT;
using System.Text.Json;

namespace Network.API.Services.Grpc
{
    public class GrpcControlService : GrpcItemControl.GrpcItemControlBase
    {
        private readonly IMqttClientService _mqttClientService;

        public GrpcControlService(MqttClientServiceProvider provider)
        {           
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
