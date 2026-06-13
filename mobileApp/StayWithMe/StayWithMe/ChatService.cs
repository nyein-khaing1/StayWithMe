using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StayWithMe;

public class ChatService
{
    private readonly HttpClient _httpClient;

    public ChatService()
    {
        // Use Windows Machine while developing the desktop version.
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:8000"),
            Timeout = TimeSpan.FromSeconds(120)
        };

        Debug.WriteLine(
            "ChatService created: http://127.0.0.1:8000"
        );
    }

    public async Task<string> SendMessageAsync(
        string characterId,
        string message)
    {
        var requestData = new
        {
            character_id = characterId,
            message
        };

        string requestJson =
            JsonSerializer.Serialize(requestData);

        Debug.WriteLine(
            $"CHAT SERVICE REQUEST: {requestJson}"
        );

        using var content = new StringContent(
            requestJson,
            Encoding.UTF8,
            "application/json"
        );

        using HttpResponseMessage response =
            await _httpClient.PostAsync("/chat", content);

        string rawResponse =
            await response.Content.ReadAsStringAsync();

        Debug.WriteLine(
            $"CHAT SERVICE STATUS: {(int)response.StatusCode}"
        );

        Debug.WriteLine(
            $"CHAT SERVICE RESPONSE: {rawResponse}"
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"FastAPI returned {(int)response.StatusCode}: " +
                rawResponse
            );
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        ChatResponse? result =
            JsonSerializer.Deserialize<ChatResponse>(
                rawResponse,
                options
            );

        if (result is null ||
            string.IsNullOrWhiteSpace(result.Reply))
        {
            throw new InvalidOperationException(
                "FastAPI returned an empty reply."
            );
        }

        return result.Reply;
    }
}

public class ChatResponse
{
    [JsonPropertyName("reply")]
    public string Reply { get; set; } = string.Empty;
}