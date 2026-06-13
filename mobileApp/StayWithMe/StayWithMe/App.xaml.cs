namespace StayWithMe;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = UserSession.IsLoggedIn
            ? new NavigationPage(new CharacterPage())
            : new NavigationPage(new LoginPage());
    }
}