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
    }

    public async Task<List<PopulationRecord>> GetPopulationDataAsync()
    {
        var url =
            $"{BaseUrl}Variables('v1111a_tot_bevolking')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<ODataResponse<PopulationRecord>>(json, options);

        return data?.Value ?? new();
    }

    public async Task<Dictionary<string, string>> GetCommuneNamesAsync()
    {
        var url = $"{BaseUrl}GeoLevels('gemeente')/GeoItems";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API 2 Response: " + json);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<ODataResponse<CommuneItem>>(json, options);

        return data?.Value.ToDictionary(x => x.ExternalCode, x => x.Name) ?? new();
    }

    private class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }
}