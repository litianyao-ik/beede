using Microsoft.Maui.Controls;
using beede;   // 引用 AppShell 所在的命名空间

namespace beede.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();   // 必须存在
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // 简单验证（可选）
            // 跳转到主页面
            Application.Current.MainPage = new AppShell();
        }
    }
}