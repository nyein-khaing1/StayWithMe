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
        if (sender is Border selectedCard)
        {
            _selectedAvatarImage = selectedCard.StyleId;

            SelectedAvatarLabel.Text = $"Selected: {_selectedAvatarImage.Replace("female_avatar_", "").Replace(".png", "").Replace("_", " ")} 💕";

            foreach (var child in ((Grid)selectedCard.Parent).Children)
            {
                if (child is Border card)
                {
                    card.Stroke = Color.FromArgb("#FFC4D6");
                    card.StrokeThickness = 2;
                }
            }

            selectedCard.Stroke = Color.FromArgb("#EA3975");
            selectedCard.StrokeThickness = 5;
        }
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_selectedAvatarImage))
        {
            await DisplayAlert("Choose avatar", "Please choose an avatar first.", "OK");
            return;
        }

        UserSession.UserAvatarImage = _selectedAvatarImage;
        UserSession.AvatarDescription = $"User selected avatar image: {_selectedAvatarImage}";
        UserSession.IsLoggedIn = true;

        Application.Current.MainPage = new NavigationPage(new CharacterPage());
    }
}