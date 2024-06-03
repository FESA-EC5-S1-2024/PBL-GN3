using Newtonsoft.Json;
using PBLprojectMVC.Models;
using PBLprojectMVC.Utils;

namespace PBLprojectMVC.Services
{
    public class TemperatureService
    {
        private readonly HttpClient _httpClient;
        private readonly string host = UtilsParams.Host();

        public TemperatureService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<TemperatureViewModel>> GetTemperatureValues(string entityName, DateTime dateFrom, DateTime dateTo, int hLimit, int hOffset)
        {
            string url = $"http://{host}:8666/STH/v2/entities/{entityName}/attrs/temperature?type=Temp&hLimit={hLimit}&hOffset={hOffset}";

            if (dateFrom != default && dateTo != default)
            {
                url += $"&dateFrom={dateFrom:yyyy-MM-ddTHH:mm:ss.fff}&dateTo={dateTo:yyyy-MM-ddTHH:mm:ss.fff}";
            }
            _httpClient.DefaultRequestHeaders.Add("fiware-service", "smart");
            _httpClient.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching temperature values: {response.StatusCode} - {response.ReasonPhrase}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var temperatureResponse = JsonConvert.DeserializeObject<TemperatureApiResponse>(jsonResponse);

            var temperatureList = new List<TemperatureViewModel>();
            foreach (var value in temperatureResponse.Value)
            {
                temperatureList.Add(new TemperatureViewModel
                {
                    Time = value.RecvTime,
                    Value = value.AttrValue
                });
            }

            return temperatureList;
        }
    }

    public class TemperatureApiResponse
    {
        public string Type { get; set; }
        public List<TemperatureValue> Value { get; set; }
    }

    public class TemperatureValue
    {
        public string Id { get; set; }
        public DateTime RecvTime { get; set; }
        public string AttrName { get; set; }
        public string AttrType { get; set; }
        public float AttrValue { get; set; }
    }
}
