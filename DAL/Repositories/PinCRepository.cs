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

    private async Task<JsonDocument> GetJsonDocument(string url)
    {
        string jsonResult = await GetData(url);
        return JsonDocument.Parse(jsonResult);
    }

    private T Deserialize<T>(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public async Task<List<PopulationRecord>> GetPopulationDataAsync()
    {
        string url =
            $"{BaseUrl}Variables('v1111a_tot_bevolking')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values";
        string json = await _httpClient.GetStringAsync(url); // Use GetStringAsync directly
        var data = Deserialize<ODataResponse<PopulationRecord>>(json);
        return data?.Value ?? new();
    }

    public async Task<Dictionary<string, string>> GetCommuneNamesAsync()
    {
        string url = BaseUrl + "GeoLevels('gemeente')/GeoItems";
        JsonDocument document = await GetJsonDocument(url);
        JsonElement root = document.RootElement;
        JsonElement valueArray = root.GetProperty("value");
        Console.WriteLine(valueArray.ToString());

        var communeNames = new Dictionary<string, string>();
        foreach (JsonElement element in valueArray.EnumerateArray())
        {
            string externalCode = element.GetProperty("ExternalCode").GetString();
            string name = element.GetProperty("Name").GetString();
            communeNames.Add(externalCode, name);
        }

        return communeNames;
    }

    public async Task<Dictionary<string, string>> GetPercentageOfMenAsync()
    {
        string url =
            $"{BaseUrl}Variables('vp1111a_mannen')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values";
        string json = await GetData(url);
        var document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        JsonElement valueArray = root.GetProperty("value");

        var percentageMenData = new Dictionary<string, string>();
        foreach (JsonElement element in valueArray.EnumerateArray())
        {
            string communeCode = element.GetProperty("ExternalCode").GetString();
            string percentageMen = element.GetProperty("ValueString").GetString();
            percentageMenData.Add(communeCode, percentageMen);
        }

        return percentageMenData;
    }

    public async Task<Dictionary<string, string>> GetHigherEducationAsync()
    {
        string url =
            $"{BaseUrl}Variables('v2390_hoog')/GeoLevels('gemeente')/PeriodLevels('year')/Periods('mrp')/Values";
        string json = await GetData(url);
        var document = JsonDocument.Parse(json);
        JsonElement root = document.RootElement;
        JsonElement valueArray = root.GetProperty("value");

        var higherEducData = new Dictionary<string, string>();
        foreach (JsonElement element in valueArray.EnumerateArray())
        {
            string communeCode = element.GetProperty("ExternalCode").GetString();
            string higherEduc = element.GetProperty("ValueString").GetString();
            higherEducData.Add(communeCode, higherEduc);
        }

        return higherEducData;
    }

    private class ODataResponse<T>
    {
        public List<T> Value { get; set; }
    }
}