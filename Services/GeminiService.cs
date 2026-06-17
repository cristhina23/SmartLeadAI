using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SmartLeadAI.Services;

public class GeminiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> ImproveNotesAsync(string notes)
    {
        var apiKey = _configuration["Gemini:ApiKey"];

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $"""
                            You are an AI assistant for SmartLead AI.

                            Your job is to improve CRM notes.

                            Rules:
                            - Return only the improved text.
                            - Do not provide multiple options.
                            - Do not use bullet points.
                            - Do not use markdown.
                            - Do not explain your changes.

                            Notes:
                            {notes}
                            """
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var response = await _httpClient.PostAsync(
            $"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={apiKey}",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return $"Gemini Error: {response.StatusCode}\n{error}";
        }

        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "No response generated.";
    }
}