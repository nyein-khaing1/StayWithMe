namespace StayWithMe;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";

        if (email == UserSession.Email && password == UserSession.Password)
        {
            UserSession.IsLoggedIn = true;
            Application.Current.MainPage = new NavigationPage(new CharacterPage());
        }
        else
        {
            await DisplayAlert("Login failed", "Email or password is incorrect.", "OK");
        }
    }

    private async void OnForgotPasswordClicked(object sender, EventArgs e)
    {
        string email = await DisplayPromptAsync(
            "Forgot Password",
            "Enter your email address:"
        );

        if (string.IsNullOrWhiteSpace(email))
            return;

        if (email.Trim() == UserSession.Email)
        {
            await DisplayAlert(
                "Password Reminder",
                $"Your password is: {UserSession.Password}",
                "OK"
            );
        }
        else
        {
            await DisplayAlert(
                "Not found",
                "No account was found with this email.",
                "OK"
            );
        }
    }
    private async void OnCreateAccountClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }



}