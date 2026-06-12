namespace StayWithMe;

public partial class CharacterPage : ContentPage
{
    public CharacterPage()
    {
        InitializeComponent();
    }

    private async void OnEthanClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage("ethan", "Ethan"));
    }

    private async void OnLeoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage("leo", "Leo"));
    }

    private async void OnNoahClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage("noah", "Noah"));
    }
}