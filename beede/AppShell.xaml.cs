using beede.Pages;
using Microsoft.Maui.Controls;

namespace beede
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes (critical)
            Routing.RegisterRoute(nameof(BillPage), typeof(BillPage));
            Routing.RegisterRoute(nameof(BillEditPage), typeof(BillEditPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MyselfPage), typeof(MyselfPage));
        }
    }
}