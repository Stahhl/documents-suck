using System.Text;
using System.Text.Json;

namespace DocumentService;

public class TemplateService
{
    private readonly ILogger<TemplateService> _logger;
    private readonly HttpClient _client;
    private readonly string _baseUrl = "http://127.0.0.1:8000";

    public TemplateService(HttpClient client, ILogger<TemplateService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<string> GetHtml(string id, object request)
    {
        var url = $"{_baseUrl}/template/{id}";
        var body = JsonSerializer.Serialize(request);

        _logger.LogInformation("body: {Body}", body);

        var response = await _client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode) return "";

        return await response.Content.ReadAsStringAsync();
    }
}