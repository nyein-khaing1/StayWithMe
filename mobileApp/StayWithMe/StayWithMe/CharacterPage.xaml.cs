namespace StayWithMe;

public partial class CharacterPage : ContentPage
{
    public CharacterPage()
    {
        InitializeComponent();
    }

    private async void OnEthanClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new MainPage(
                characterId: "ethan",
                characterName: "Ethan",
                themeColor: "#EA3975",
                avatarImage: "ethan_profile.png",
                backgroundImage: "ethan_chat_background.png"
            )
        );
    }

    private async void OnLeoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new MainPage(
                characterId: "leo",
                characterName: "Leo",
                themeColor: "#F58A00",
                avatarImage: "leo_profile.png",
                backgroundImage: "leo_chat_background.png"
            )
        );
    }

    private async void OnNoahClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new MainPage(
                characterId: "noah",
                characterName: "Noah",
                themeColor: "#2575D9",
                avatarImage: "noah_profile.png",
                backgroundImage: "noah_chat_background.png"
            )
        );
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

   
}

