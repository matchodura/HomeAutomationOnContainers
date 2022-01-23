using Entities.DHT;
using System.Text.Json;

namespace HomeControl.WebApp.Data
{
    public class SensorService
    {
        private readonly IHttpClientFactory _clientFactory;


        public SensorService(IHttpClientFactory clientFactory)
        {
                _clientFactory = clientFactory;
        }

        //TODO use a client for it
        public async Task<List<DHT>> GetSensorData()
        {

            // var request = new HttpRequestMessage(HttpMethod.Get, "localhost:6000/api/v1/sensor/values");
            var request = "https://localhost:6001/api/v1/sensor/sensors";

       
            var httpClient = _clientFactory.CreateClient();

            using var httpResponse = await httpClient.GetAsync(request);


            httpResponse.EnsureSuccessStatusCode();

            var contents = await httpResponse.Content.ReadAsStringAsync();


            var result = JsonSerializer.Deserialize<List<DHT>>(contents);


            return result;

            //if (response.IsSuccessStatusCode)
            //{
            //   using var responseStream = await response.Content.ReadAsStreamAsync();
            //    branches = await JsonSerializer.DeserializeAsync<IEnumerable<GitHubBranch>>(responseStream);

            //}

        }
    }
}


