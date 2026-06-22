namespace StayWithMe;

public static class UserSession
{
    public static bool IsLoggedIn
    {
        get => Preferences.Get("is_logged_in", false);
        set => Preferences.Set("is_logged_in", value);
    }

    public static string Name
    {
        get => Preferences.Get("name", "");
        set => Preferences.Set("name", value);
    }

    public static string Email
    {
        get => Preferences.Get("email", "");
        set => Preferences.Set("email", value);
    }

    public static string Password
    {
        get => Preferences.Get("password", "");
        set => Preferences.Set("password", value);
    }

    public static string AvatarDescription
    {
        get => Preferences.Get("avatar_description", "");
        set => Preferences.Set("avatar_description", value);
    }

    public static string UserAvatarImage
    {
        get => Preferences.Get("user_avatar_image", "");
        set => Preferences.Set("user_avatar_image", value);
    }

    public static void Logout()
    {
        IsLoggedIn = false;
    }

    public static void ClearAll()
    {
        Preferences.Clear();
    }

    // Relationship points per boyfriend
    public static int GetRelationshipPoints(string characterId)
    {
        return Preferences.Get($"relationship_points_{characterId}", 0);
    }

    public static void AddRelationshipPoints(string characterId, int points)
    {
        int currentPoints = GetRelationshipPoints(characterId);

        Preferences.Set(
            $"relationship_points_{characterId}",
            currentPoints + points
        );
    }

    public static string GetRelationshipLevel(string characterId)
    {
        int points = GetRelationshipPoints(characterId);

        if (points >= 2000) return "Soulmate 💍";
        if (points >= 1000) return "Special Person 💕";
        if (points >= 500) return "Close Friend 💖";
        if (points >= 100) return "Friend 😊";

        return "New Connection ✨";
    }

    // Chat history per boyfriend
    public static string GetChatHistory(string characterId)
    {
        return Preferences.Get($"chat_history_{characterId}", "");
    }

    public static void SaveChatMessage(
        string characterId,
        string characterName,
        string userMessage,
        string aiReply)
    {
        string oldHistory = GetChatHistory(characterId);

        string newMessage =
            $"\n\n[{DateTime.Now:dd/MM/yyyy HH:mm}]\nYou: {userMessage}\n{characterName}: {aiReply}";

        Preferences.Set(
            $"chat_history_{characterId}",
            oldHistory + newMessage
        );
    }

    public static void ClearChatHistory(string characterId)
    {
        Preferences.Remove($"chat_history_{characterId}");
    }

    // Memory per boyfriend
    public static string GetMemory(string characterId)
    {
        return Preferences.Get($"memory_{characterId}", "");
    }

    public static void AddMemory(string characterId, string memory)
    {
        string oldMemory = GetMemory(characterId);

        if (!oldMemory.Contains(memory))
        {
            Preferences.Set(
                $"memory_{characterId}",
                oldMemory + "\n- " + memory
            );
        }
    }

    public static void ClearMemory(string characterId)
    {
        Preferences.Remove($"memory_{characterId}");
    }

    public static string TodayMood
    {
        get => Preferences.Get("today_mood", "");
        set => Preferences.Set("today_mood", value);
    }

    public static string LastMoodDate
    {
        get => Preferences.Get("last_mood_date", "");
        set => Preferences.Set("last_mood_date", value);
    }

    public static bool HasCheckedMoodToday()
    {
        return LastMoodDate == DateTime.Today.ToString("yyyy-MM-dd");
    }

}