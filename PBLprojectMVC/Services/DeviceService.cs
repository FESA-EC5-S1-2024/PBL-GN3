using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

public class DeviceService
{
    private readonly HttpClient _httpClient;

    public DeviceService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> RegisterDeviceAsync(string host, int port, string deviceId, string entityName)
    {
        var url = $"http://{host}:{port}/iot/devices";
        var payload = new
        {
            devices = new[]
            {
                new
                {
                    device_id = deviceId,
                    entity_name = entityName,
                    entity_type = "Temp",
                    protocol = "PDI-IoTA-UltraLight",
                    transport = "MQTT",
                    attributes = new[]
                    {
                        new { object_id = "t", name = "temperature", type = "Float" }
                    }
                }
            }
        };

        var json = JsonConvert.SerializeObject(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        content.Headers.Add("fiware-service", "smart");
        content.Headers.Add("fiware-servicepath", "/");

        var response = await _httpClient.PostAsync(url, content);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteDeviceAsync(string host, int port, string deviceId)
    {
        var url = $"http://{host}:{port}/iot/devices/{deviceId}";

        var request = new HttpRequestMessage(HttpMethod.Delete, url);

        request.Headers.Add("fiware-service", "smart");
        request.Headers.Add("fiware-servicepath", "/");

        var response = await _httpClient.SendAsync(request);

        return response;
    }
}
