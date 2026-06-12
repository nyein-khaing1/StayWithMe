using Microsoft.Maui.Controls.Shapes;

namespace StayWithMe;

public partial class MainPage : ContentPage
{
    private readonly ChatService _chatService = new ChatService();
    private bool _isSending = false;

    private string _characterId;
    private string _characterName;

    public MainPage(string characterId, string characterName)
    {
        InitializeComponent();

        _characterId = characterId;
        _characterName = characterName;

        OnlineLabel.Text = $"{_characterName} is online";

        AddMessage(_characterName, GetOpeningMessage());
    }

    private string GetOpeningMessage()
    {
        return _characterId switch
        {
            "ethan" => "Hey, I’m here. Tell me what’s on your mind.",
            "leo" => "Well, look who came to talk to me. I missed you.",
            "noah" => "Hi. Take your time, I’m listening.",
            _ => "Hey, I’m here with you."
        };
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        await SendMessage();
    }

    private async void OnUserInputCompleted(object sender, EventArgs e)
    {
        await SendMessage();
    }

    private async void OnChangeCharacterClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async Task SendMessage()
    {
        if (_isSending)
            return;

        string message = UserInput.Text;

        if (string.IsNullOrWhiteSpace(message))
            return;

        _isSending = true;
        UserInput.Text = "";

        AddMessage("You", message);

        string reply = await _chatService.SendMessageAsync(_characterId, message);

        AddMessage(_characterName, reply);

        _isSending = false;
    }

    private async void AddMessage(string sender, string message)
    {
        bool isUser = sender == "You";

        var bubble = new Border
        {
            BackgroundColor = isUser ? Colors.White : Color.FromArgb("#FFE1EC"),
            Stroke = isUser ? Color.FromArgb("#EEEEEE") : Color.FromArgb("#FFD1DC"),
            StrokeThickness = 1,
            Padding = 12,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 18
            },
            HorizontalOptions = isUser ? LayoutOptions.End : LayoutOptions.Start,
            MaximumWidthRequest = 320,
            Content = new Label
            {
                Text = $"{sender}: {message}",
                FontSize = 15,
                TextColor = Colors.Black,
                LineBreakMode = LineBreakMode.WordWrap
            }
        };

        ChatContainer.Children.Add(bubble);

        await Task.Delay(100);
        await ChatScroll.ScrollToAsync(ChatContainer, ScrollToPosition.End, true);
    }
}