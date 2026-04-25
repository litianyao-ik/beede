using Microsoft.Maui.Controls;
using beede.Services;

namespace beede.Pages;

public partial class LoginPage : ContentPage
{
    private readonly UserSessionService _session = new();

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UserNameEntry?.Text?.Trim();
        var password = PasswordEntry?.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await DisplayAlertAsync("Notice", "Please enter username and password", "OK");
            return;
        }

        if (_session.Login(username, password))
        {
            // Navigate to main page
            Application.Current.MainPage = new AppShell();
            return;
        }

        var result = await DisplayAlertAsync("Notice", "User does not exist. Register new user?", "Register", "Cancel");
        if (result)
        {
            if (_session.RegisterUser(username, password))
            {
                _session.Login(username, password);
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlertAsync("Failed", "Registration failed. Please try again.", "OK");
            }
        }
    }
}