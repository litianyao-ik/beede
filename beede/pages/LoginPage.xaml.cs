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
            await DisplayAlertAsync("提示", "请输入用户名和密码", "OK");
            return;
        }

        if (_session.Login(username, password))
        {
            // 使用新方式切换页面
            Application.Current.MainPage = new AppShell();
            return;
        }

        var result = await DisplayAlertAsync("提示", "用户不存在，是否注册新用户？", "注册", "取消");
        if (result)
        {
            if (_session.RegisterUser(username, password))
            {
                _session.Login(username, password);
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await DisplayAlertAsync("失败", "注册失败，请重试", "OK");
            }
        }
    }
}