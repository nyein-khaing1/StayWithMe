namespace StayWithMe;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        LoadUserProfile();
    }

    private void LoadUserProfile()
    {
        UserInfoLabel.Text =
            $"Welcome back, {UserSession.Name} 💕";

        if (!string.IsNullOrWhiteSpace(UserSession.UserAvatarImage))
        {
            UserAvatarImage.Source = UserSession.UserAvatarImage;
        }
    }

    private async void OnChangeAvatarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AvatarCreatorPage());
    }

    private async void OnChangePasswordClicked(object sender, EventArgs e)
    {
        string newPassword = NewPasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(newPassword))
        {
            await DisplayAlert("Missing password", "Please enter a new password.", "OK");
            return;
        }

        UserSession.Password = newPassword;

        await DisplayAlert("Updated", "Your password has been changed.", "OK");

        NewPasswordEntry.Text = "";
    }

    private async void OnChatHistoryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChatHistoryPage());
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        UserSession.Logout();

        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }

    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}