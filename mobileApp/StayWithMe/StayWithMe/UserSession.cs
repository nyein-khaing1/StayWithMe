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
}