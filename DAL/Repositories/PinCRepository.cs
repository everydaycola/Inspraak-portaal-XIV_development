using System.Net.Http.Headers;
using System.Text.Json;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace DAL.Repositories;

public class PinCRepository : IPinCRepository
{
    private readonly string _apiKey;
    private const string BaseUrl = "https://provincies.incijfers.be/jiveservices/odata/";
    private readonly HttpClient _httpClient;

    public PinCRepository()
    {
        _apiKey = Environment.GetEnvironmentVariable("PINC_API_KEY");
        if (string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("API key is not set in environment variables.");
        }

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task<string> GetData(string url)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<Dictionary<string, string>> GetKeyValuePairsFromEndpointAsync(string url, string keyPropertyName,
        string valuePropertyName)
    {
        string jsonResult = await GetData(url);
        // just some JSON magic for getting the data from the API
        JsonDocument document = JsonDocument.Parse(jsonResult);
        JsonElement root = document.RootElement;
        JsonElement valueArray = root.GetProperty("value");

        // We'll collect 2 values key = the Code of the 'Gemeente' and the value which depends on what we put in the filter
        var data = new Dictionary<string, string>();
        foreach (JsonElement element in valueArray.EnumerateArray())
        {
            string communeCode = element.GetProperty(keyPropertyName).GetString();
            string value = element.GetProperty(valuePropertyName).GetString();
            data.Add(communeCode, value);
        }

        return data;
    }


    public async Task<Dictionary<string, string>> GetCommuneNamesAsync()
    {
        string url = BaseUrl + "GeoLevels('gemeente')/GeoItems";
        return await GetKeyValuePairsFromEndpointAsync(url, "ExternalCode", "Name");
    }

    public async Task<Dictionary<string, string>> GetDataFromAPI(string filter)
    {
        // This is the endpoint for the API
        string url =
            $"{BaseUrl}Variables('{filter}')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values";
        return await GetKeyValuePairsFromEndpointAsync(url, "ExternalCode", "ValueString");
    }
}