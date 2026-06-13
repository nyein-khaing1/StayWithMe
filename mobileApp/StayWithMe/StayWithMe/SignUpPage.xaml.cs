namespace StayWithMe;

public partial class SignUpPage : ContentPage
{
    public SignUpPage()
    {
        InitializeComponent();
    }

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";
        string gender = GenderPicker.SelectedItem?.ToString() ?? "";

        int age = CalculateAge(DobPicker.Date ?? DateTime.Today);

        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(gender))
        {
            await DisplayAlert("Missing details", "Please fill in all fields.", "OK");
            return;
        }

        if (gender != "Female")
        {
            await DisplayAlert("Not eligible", "This app is only available for female users.", "OK");
            return;
        }

        if (age < 18)
        {
            await DisplayAlert("Not eligible", "You must be 18 or over to use this app.", "OK");
            return;
        }

        UserSession.Name = name;
        UserSession.Email = email;
        UserSession.Password = password;

        await Navigation.PushAsync(new AvatarCreatorPage());
    }

    private int CalculateAge(DateTime dob)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-age))
            age--;

        return age;
    }
}