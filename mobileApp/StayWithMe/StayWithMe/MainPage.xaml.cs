using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System.Diagnostics;

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

    private void OnPageLoaded(object? sender, EventArgs e)
    {
        if (_openingMessageAdded)
            return;

        _openingMessageAdded = true;

        LoadPreviousChatMessages();

        if (ChatContainer.Children.Count == 0)
        {
            AddMessage(_characterName, GetOpeningMessage());
        }
    }


    private void LoadPreviousChatMessages()
    {
        string history = UserSession.GetChatHistory(_characterId);

        if (string.IsNullOrWhiteSpace(history))
            return;

        string[] lines = history.Split('\n');

        foreach (string line in lines)
        {
            if (line.StartsWith("You: "))
            {
                AddMessage("You", line.Replace("You: ", ""));
            }
            else if (line.StartsWith($"{_characterName}: "))
            {
                AddMessage(_characterName, line.Replace($"{_characterName}: ", ""));
            }
        }
    }

    private async void OnClearChatClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "Delete chat",
            $"Delete your chat history with {_characterName}?",
            "Delete",
            "Cancel"
        );

        if (!confirm)
            return;

        UserSession.ClearChatHistory(_characterId);
        

        ChatContainer.Children.Clear();

        AddMessage(_characterName, GetOpeningMessage());
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
        RelationshipLabel.TextColor = theme;

        HeaderBorder.Stroke = theme;
        InputBorder.Stroke = theme;
        SendButton.BackgroundColor = theme;

        TypingLabel.Text = $"{_characterName} is typing...";
        TypingLabel.IsVisible = false;

        UpdateRelationshipLabel();
    }

    private void UpdateRelationshipLabel()
    {
        int points = UserSession.GetRelationshipPoints(_characterId);
        string level = UserSession.GetRelationshipLevel(_characterId);

        RelationshipLabel.Text = $"💕 {level} • {points} points";
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
            "ethan" => "Hey love, I’m here. How has your day been?",
            "leo" => "Well, look who finally came to talk to me 😌 What are you doing?",
            "noah" => "You’re here. Come sit with me and tell me what’s on your mind.",
            _ => "Hey, I’m here with you."
        };
    }


    private void CheckAndSaveMemory(string message)
    {
        string lowerMessage = message.ToLower();

        if (lowerMessage.Contains("my favourite food is") ||
            lowerMessage.Contains("my favorite food is"))
        {
            UserSession.AddMemory(
                _characterId,
                $"User's favourite food: {message}"
            );
        }

        if (lowerMessage.Contains("my birthday is"))
        {
            UserSession.AddMemory(
                _characterId,
                $"User's birthday: {message}"
            );
        }

        if (lowerMessage.Contains("i study") ||
            lowerMessage.Contains("i am studying"))
        {
            UserSession.AddMemory(
                _characterId,
                $"User studies: {message}"
            );
        }

        if (lowerMessage.Contains("i like") ||
            lowerMessage.Contains("i love"))
        {
            UserSession.AddMemory(
                _characterId,
                $"User likes/loves: {message}"
            );
        }
    }

    private async void OnSendClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("SEND BUTTON CLICKED");
        await SendCurrentMessageAsync();
    }

    private async void OnUserInputCompleted(object sender, EventArgs e)
    {
        Debug.WriteLine("ENTER KEY PRESSED");
        await SendCurrentMessageAsync();
    }

    private async Task SendCurrentMessageAsync()
    {
        if (_isSending)
        {
            Debug.WriteLine("Message ignored because another message is sending.");
            return;
        }

        string message = UserInput.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(message))
        {
            Debug.WriteLine("Message ignored because it is empty.");
            return;
        }

        _isSending = true;

        UserInput.Text = string.Empty;
        UserInput.IsEnabled = false;
        SendButton.IsEnabled = false;

        AddMessage("You", message);

        CheckAndSaveMemory(message);

        TypingLabel.Text =
            $"{_characterName} is typing...";
        TypingLabel.IsVisible = true;

        try
        {
            Debug.WriteLine("CALLING FASTAPI NOW");
            Debug.WriteLine($"Character ID: {_characterId}");
            Debug.WriteLine($"User message: {message}");

            string reply = await _chatService.SendMessageAsync(
                _characterId,
                message
            );

            Debug.WriteLine($"MAIN PAGE RECEIVED: {reply}");

            TypingLabel.IsVisible = false;

            AddMessage(_characterName, reply);

            UserSession.AddRelationshipPoints(_characterId, 5);
            UpdateRelationshipLabel();

            UserSession.SaveChatMessage(
                    _characterId,
                    _characterName,
                    message,
                    reply
                );
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CHAT REQUEST FAILED: {ex}");

            TypingLabel.IsVisible = false;

            AddMessage(
                _characterName,
                $"Connection error: {ex.Message}"
            );
        }
        finally
        {
            _isSending = false;

            UserInput.IsEnabled = true;
            SendButton.IsEnabled = true;

            UserInput.Focus();
        }
    }

    private void AddMessage(string sender, string message)
    {
        bool isUser = sender == "You";
        Color theme = Color.FromArgb(_themeColor);

        var senderLabel = new Label
        {
            Text = sender,
            FontSize = 12,
            FontAttributes = FontAttributes.Bold,
            TextColor = isUser ? Colors.White : theme
        };

        var messageLabel = new Label
        {
            Text = message,
            FontSize = 16,
            TextColor = isUser ? Colors.White : Color.FromArgb("#29232D"),
            LineBreakMode = LineBreakMode.WordWrap
        };

        var messageContent = new VerticalStackLayout
        {
            Spacing = 4
        };

        messageContent.Children.Add(senderLabel);
        messageContent.Children.Add(messageLabel);

        var bubble = new Border
        {
            BackgroundColor = isUser ? theme : Colors.White,
            Stroke = isUser ? theme : Color.FromArgb("#DDD6DF"),
            StrokeThickness = 1,
            Padding = new Thickness(15, 11),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 20
            },
            MaximumWidthRequest = 440,
            Content = messageContent
        };

        if (isUser)
        {
            var userAvatar = new Image
            {
                Source = UserSession.UserAvatarImage,
                WidthRequest = 42,
                HeightRequest = 42,
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.Start
            };

            userAvatar.Clip = new EllipseGeometry
            {
                Center = new Point(21, 21),
                RadiusX = 21,
                RadiusY = 21
            };

            var userRow = new HorizontalStackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.End,
                MaximumWidthRequest = 510
            };

            bubble.HorizontalOptions = LayoutOptions.End;

            userRow.Children.Add(bubble);
            userRow.Children.Add(userAvatar);

            ChatContainer.Children.Add(userRow);

            return;
        }

        Image avatar = CreateMessageAvatar();

        var replyRow = new HorizontalStackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            MaximumWidthRequest = 510
        };

        bubble.HorizontalOptions = LayoutOptions.Start;

        replyRow.Children.Add(avatar);
        replyRow.Children.Add(bubble);

        ChatContainer.Children.Add(replyRow);
    }

    private Image CreateMessageAvatar()
    {
        var avatar = new Image
        {
            Source = _avatarImage,
            WidthRequest = 42,
            HeightRequest = 42,
            Aspect = Aspect.AspectFill,
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Start
        };

        avatar.Clip = new EllipseGeometry
        {
            Center = new Point(21, 21),
            RadiusX = 21,
            RadiusY = 21
        };


        return avatar;
    }

    private async void OnChangeCharacterClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnMemoriesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new MemoryPage(_characterId, _characterName)
        );
    }

    private async void OnChatMenuClicked(object sender, EventArgs e)
    {
        string action = await DisplayActionSheet(
            $"{_characterName}",
            "Cancel",
            null,
            "💭 Memories",
            "🗑 Clear Chat",
            "🔄 Change Character"
        );

        switch (action)
        {
            case "💭 Memories":
                await Navigation.PushAsync(
                    new MemoryPage(
                        _characterId,
                        _characterName
                    ));
                break;

            case "🗑 Clear Chat":
                OnClearChatClicked(sender, e);
                break;

            case "🔄 Change Character":
                await Navigation.PopAsync();
                break;
        }
    }
}