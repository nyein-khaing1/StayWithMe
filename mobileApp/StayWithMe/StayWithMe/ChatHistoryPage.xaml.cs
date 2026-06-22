namespace StayWithMe;

public partial class ChatHistoryPage : ContentPage
{
    public ChatHistoryPage()
    {
        InitializeComponent();

        LoadHistory();
    }

    private void LoadHistory()
    {
        string ethanHistory = UserSession.GetChatHistory("ethan");
        string leoHistory = UserSession.GetChatHistory("leo");
        string noahHistory = UserSession.GetChatHistory("noah");

        if (string.IsNullOrWhiteSpace(ethanHistory) &&
            string.IsNullOrWhiteSpace(leoHistory) &&
            string.IsNullOrWhiteSpace(noahHistory))
        {
            HistoryLabel.Text = "No chat history yet 💕";
            return;
        }

        HistoryLabel.Text =
            $"💗 Ethan\n{FormatHistory(ethanHistory)}\n\n" +
            $"🧡 Leo\n{FormatHistory(leoHistory)}\n\n" +
            $"💙 Noah\n{FormatHistory(noahHistory)}";
    }

    private string FormatHistory(string history)
    {
        if (string.IsNullOrWhiteSpace(history))
            return "No chat history yet.";

        return history.Trim();
    }

    private async void OnClearHistoryClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Clear all history",
            "Are you sure you want to delete all chat history for Ethan, Leo, and Noah?",
            "Yes",
            "No"
        );

        if (!confirm)
            return;

        UserSession.ClearChatHistory("ethan");
        UserSession.ClearChatHistory("leo");
        UserSession.ClearChatHistory("noah");

        LoadHistory();

        await DisplayAlert(
            "Deleted",
            "All chat history has been cleared.",
            "OK"
        );
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}