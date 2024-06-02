using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PBLprojectMVC.Models;
using System.Net.Http.Headers;
using System.Text;

public class DeviceService
{
    private readonly HttpClient _httpClient;
    private readonly string host = "your_host";

    public DeviceService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> RegisterDeviceAsync(int port, string deviceId, string entityName)
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

    public async Task<HttpResponseMessage> DeleteDeviceAsync(int port, string deviceId)
    {
        var url = $"http://{host}:{port}/iot/devices/{deviceId}";

        var request = new HttpRequestMessage(HttpMethod.Delete, url);

        request.Headers.Add("fiware-service", "smart");
        request.Headers.Add("fiware-servicepath", "/");

        var response = await _httpClient.SendAsync(request);

        return response;
    }

    public async Task<List<DeviceViewModel>> GetDevicesList(int port)
    {
        var url = $"http://{host}:{port}/iot/devices";
        _httpClient.DefaultRequestHeaders.Add("fiware-service", "smart");
        _httpClient.DefaultRequestHeaders.Add("fiware-servicepath", "/");

        HttpResponseMessage response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error fetching devices: {response.StatusCode} - {response.ReasonPhrase}");
        }

        string jsonResponse = await response.Content.ReadAsStringAsync();
        var deviceResponse = JsonConvert.DeserializeObject<DeviceApiResponse>(jsonResponse);

        return deviceResponse.Devices.Select(device => new DeviceViewModel
        {
            Name = device.device_id,
            Entity = device.entity_name,
            Type = device.entity_type,
            Transport = device.transport
        }).ToList();
    }

    public class DeviceApiResponse
    {
        public int Count { get; set; }
        public List<Device> Devices { get; set; }
    }
    public class Device
    {
        public string device_id { get; set; }
        public string entity_name { get; set; }
        public string entity_type { get; set; }
        public string transport { get; set; }
    }
}
