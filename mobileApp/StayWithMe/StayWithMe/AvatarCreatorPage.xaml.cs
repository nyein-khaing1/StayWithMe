namespace StayWithMe;

public partial class AvatarCreatorPage : ContentPage
{
    private string _selectedAvatarImage = "";

    public AvatarCreatorPage()
    {
        InitializeComponent();
    }

    private void OnAvatarSelected(object sender, TappedEventArgs e)
    {
        if (sender is not Border selectedCard)
            return;

        _selectedAvatarImage = selectedCard.StyleId ?? "";

        if (string.IsNullOrWhiteSpace(_selectedAvatarImage))
            return;

        SelectedAvatarLabel.Text = "Avatar selected 💕";

        ResetAvatarCards();

        selectedCard.Stroke = Color.FromArgb("#EA3975");
        selectedCard.StrokeThickness = 5;
    }

    private void ResetAvatarCards()
    {
        AsianCard.Stroke = Color.FromArgb("#FFC4D6");
        AsianCard.StrokeThickness = 2;

        WhiteCard.Stroke = Color.FromArgb("#FFC4D6");
        WhiteCard.StrokeThickness = 2;

        BlackCard.Stroke = Color.FromArgb("#FFC4D6");
        BlackCard.StrokeThickness = 2;

        LatinaCard.Stroke = Color.FromArgb("#FFC4D6");
        LatinaCard.StrokeThickness = 2;

        MiddleEasternCard.Stroke = Color.FromArgb("#FFC4D6");
        MiddleEasternCard.StrokeThickness = 2;
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_selectedAvatarImage))
        {
            await DisplayAlert(
                "Choose avatar",
                "Please choose an avatar first.",
                "OK"
            );
            return;
        }

        UserSession.UserAvatarImage = _selectedAvatarImage;
        UserSession.AvatarDescription =
            $"User selected avatar image: {_selectedAvatarImage}";

        UserSession.IsLoggedIn = true;

        Application.Current.MainPage =
            new NavigationPage(new CharacterPage());
    }
}