namespace StayWithMe;

public partial class MemoryPage : ContentPage
{
    private readonly string _characterId;
    private readonly string _characterName;

    public MemoryPage(string characterId, string characterName)
    {
        InitializeComponent();

        _characterId = characterId;
        _characterName = characterName;

        TitleLabel.Text = $"{_characterName}'s Memories 💕";

        LoadMemories();
    }

    private void LoadMemories()
    {
        string memories = UserSession.GetMemory(_characterId);

        if (string.IsNullOrWhiteSpace(memories))
        {
            MemoryLabel.Text =
                $"{_characterName} has not saved any memories yet.";
        }
        else
        {
            MemoryLabel.Text = memories.Trim();
        }
    }

    private async void OnClearMemoriesClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Clear memories",
            $"Delete {_characterName}'s memories about you?",
            "Delete",
            "Cancel"
        );

        if (!confirm)
            return;

        UserSession.ClearMemory(_characterId);

        LoadMemories();
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}