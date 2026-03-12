using Microsoft.Maui.Controls;

namespace beede.Pages
{
    public partial class MyselfPage : ContentPage
    {
        public MyselfPage()
        {
            InitializeComponent();   // 必须存在
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}