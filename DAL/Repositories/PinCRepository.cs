using System.Net.Http.Headers;
using System.Text.Json;
using DAL.Interfaces;
using Domain.CitizenPanel;

namespace DAL.Repositories;

public class PinCRepository : IPinCRepository
{
    private const string BaseUrl = "https://provincies.incijfers.be/jiveservices/odata/";
    private readonly HttpClient _httpClient;

    public PinCRepository()
    {
        var apiKey = Environment.GetEnvironmentVariable("PINC_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key is not set in environment variables.");
        }

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("apikey", apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    private async Task<string> GetData(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<Dictionary<string, string>> GetKeyValuePairsFromEndpointAsync(string url, string keyPropertyName,
        string valuePropertyName)
    {
        var jsonResult = await GetData(url);
        // just some JSON magic for getting the data from the API
        var document = JsonDocument.Parse(jsonResult);
        var root = document.RootElement;
        var valueArray = root.GetProperty("value");

        // We'll collect 2 values key = the Code of the 'Gemeente' and the value which depends on what we put in the filter

        return valueArray.EnumerateArray().ToDictionary(element => element.GetProperty(keyPropertyName).GetString(), element => element.GetProperty(valuePropertyName).GetString());
    }
    
    public async Task<Dictionary<string, string>> GetCommuneNamesAsync()
    {
        const string url = BaseUrl + "GeoLevels('gemeente')/GeoItems";
        return await GetKeyValuePairsFromEndpointAsync(url, "ExternalCode", "Name");
    }

    public async Task<Dictionary<string, string>> GetDataFromApi(string filter)
    {
        // This is the endpoint for the API
        return await GetKeyValuePairsFromEndpointAsync(
            $"{BaseUrl}Variables('{filter}')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values",
            "ExternalCode",
            "ValueString"
            );
    }
}