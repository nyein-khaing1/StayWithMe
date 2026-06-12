using System.Net.Http.Json;

namespace StayWithMe;

public class ChatService
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("http://127.0.0.1:8000")
    };

    public async Task<string> SendMessageAsync(string message)
    {
        var request = new
        {
            message = message
        };

        var response = await _httpClient.PostAsJsonAsync("/chat", request);

        var result = await response.Content.ReadFromJsonAsync<ChatResponse>();

        return result?.reply ?? "I’m here with you.";
    }
}

public class ChatResponse
{
    public string reply { get; set; }
}