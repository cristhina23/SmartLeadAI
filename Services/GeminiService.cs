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

    public async Task<string> GenerateSummaryAsync(string text)
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

                            Create a concise professional business summary of the following conversation, meeting notes, customer interaction, or communication.

                            Rules:
                            - Return only the summary.
                            - Do not use bullet points.
                            - Do not use markdown.
                            - Keep it concise and professional.
                            - Write as if it will be stored in a CRM system.

                            Content:
                            {text}
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

    public async Task<string> SuggestResponseAsync(string text)
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

                            Generate a professional response to the following customer message.

                            Rules:
                            - Return only the response.
                            - Be professional and friendly.
                            - Do not use markdown.
                            - Do not explain your reasoning.
                            - The response should be ready to send directly to a customer.

                            Customer Message:
                            {text}
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

