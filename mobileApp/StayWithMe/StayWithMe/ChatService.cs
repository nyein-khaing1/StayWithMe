using System.Net.Http.Json;

namespace StayWithMe;

public class ChatService
{
    private readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("http://127.0.0.1:8000")
    };

    public async Task<string> SendMessageAsync(string characterId, string message)
    {
        try
        {
            var request = new
            {
                character_id = characterId,
                message = message
            };

            var response = await _httpClient.PostAsJsonAsync("/chat", request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                return $"Backend error: {response.StatusCode} - {error}";
            }

            var result = await response.Content.ReadFromJsonAsync<ChatResponse>();

            return result?.reply ?? "I’m here with you.";
        }
        catch (Exception ex)
        {
            return "Connection error: " + ex.Message;
        }
    }
}

public class ChatResponse
{
    public string reply { get; set; } = "";
}