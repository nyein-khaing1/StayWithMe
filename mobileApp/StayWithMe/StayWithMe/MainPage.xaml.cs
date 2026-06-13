using Microsoft.Maui.Controls.Shapes;

namespace StayWithMe;

public partial class MainPage : ContentPage
{
    private readonly ChatService _chatService = new();

    private readonly string _characterId;
    private readonly string _characterName;
    private readonly string _themeColor;
    private readonly string _avatarImage;
    private readonly string _backgroundImage;

    private bool _isSending;
    private bool _openingMessageAdded;

    public MainPage(
        string characterId,
        string characterName,
        string themeColor,
        string avatarImage,
        string backgroundImage)
    {
        InitializeComponent();

        _characterId = characterId;
        _characterName = characterName;
        _themeColor = themeColor;
        _avatarImage = avatarImage;
        _backgroundImage = backgroundImage;

        SetupCharacterStyle();

        Loaded += OnPageLoaded;
    }

    private async void OnPageLoaded(object? sender, EventArgs e)
    {
        if (_openingMessageAdded)
            return;

        _openingMessageAdded = true;

        await AddMessageAsync(
            _characterName,
            GetOpeningMessage()
        );
    }

    private void SetupCharacterStyle()
    {
        Color theme = Color.FromArgb(_themeColor);

        ChatBackgroundImage.Source = _backgroundImage;

        AvatarImage.Source = _avatarImage;
        ChatAvatarImage.Source = _avatarImage;

        NameLabel.Text = _characterName;
        CharacterNameLabel.Text = _characterName;
        PersonalityLabel.Text = GetPersonality();

        NameLabel.TextColor = theme;
        CharacterNameLabel.TextColor = theme;

        HeaderBorder.Stroke = theme;
        InputBorder.Stroke = theme;
        SendButton.BackgroundColor = theme;

        TypingLabel.Text = $"{_characterName} is typing...";
    }

    private string GetPersonality()
    {
        return _characterId switch
        {
            "ethan" => "Soft • Caring • Protective",
            "leo" => "Bubbly • Playful • Flirty",
            "noah" => "Calm • Mature • Confident",
            _ => "Always here for you"
        };
    }

    private string GetOpeningMessage()
    {
        return _characterId switch
        {
            "ethan" =>
                "Hey love, I’m here. How has your day been?",

            "leo" =>
                "Well, look who finally came to talk to me 😌 What are you doing?",

            "noah" =>
                "You’re here. Come sit with me and tell me what’s on your mind.",

            _ =>
                "Hey, I’m here with you."
        };
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        await SendMessageAsync();
    }

    private async void OnUserInputCompleted(object sender, EventArgs e)
    {
        await SendMessageAsync();
    }

    private async Task SendMessageAsync()
    {
        if (_isSending)
            return;

        string message = UserInput.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(message))
            return;

        _isSending = true;
        SendButton.IsEnabled = false;
        UserInput.IsEnabled = false;

        UserInput.Text = "";

        await AddMessageAsync("You", message);

        TypingLabel.IsVisible = true;

        try
        {
            string reply = await _chatService.SendMessageAsync(
                _characterId,
                message
            );

            TypingLabel.IsVisible = false;

            await AddMessageAsync(
                _characterName,
                reply
            );
        }
        catch (Exception ex)
        {
            TypingLabel.IsVisible = false;

            await AddMessageAsync(
                _characterName,
                $"I couldn’t reply just now. {ex.Message}"
            );
        }
        finally
        {
            _isSending = false;
            SendButton.IsEnabled = true;
            UserInput.IsEnabled = true;
            UserInput.Focus();
        }
    }

    private async Task AddMessageAsync(
        string sender,
        string message)
    {
        bool isUser = sender == "You";
        Color theme = Color.FromArgb(_themeColor);

        var senderLabel = new Label
        {
            Text = sender,
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = isUser
                ? Colors.White
                : theme
        };

        var messageLabel = new Label
        {
            Text = message,
            FontSize = 16,
            TextColor = isUser
                ? Colors.White
                : Color.FromArgb("#29232D"),
            LineBreakMode = LineBreakMode.WordWrap
        };

        var messageContent = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                senderLabel,
                messageLabel
            }
        };

        var bubble = new Border
        {
            BackgroundColor = isUser
                ? theme
                : Color.FromArgb("#F9FFFFFF"),

            Stroke = isUser
                ? theme
                : Color.FromArgb("#DDD6DF"),

            StrokeThickness = 1,
            Padding = new Thickness(15, 11),

            StrokeShape = new RoundRectangle
            {
                CornerRadius = 21
            },

            HorizontalOptions = isUser
                ? LayoutOptions.End
                : LayoutOptions.Start,

            MaximumWidthRequest = 455,
            Content = messageContent
        };

        ChatContainer.Children.Add(bubble);

        await Task.Delay(70);

        await ChatScroll.ScrollToAsync(
            ChatContainer,
            ScrollToPosition.End,
            true
        );
    }

    private async void OnChangeCharacterClicked(
        object sender,
        EventArgs e)
    {
        await Navigation.PopAsync();
    }
}