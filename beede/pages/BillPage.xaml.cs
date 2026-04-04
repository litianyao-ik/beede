using Microsoft.Maui.Controls;

namespace beede.Pages
{
    public partial class BillPage : ContentPage
    {
        public BillPage()
        {
            InitializeComponent();
        }

        private void OnCalculateClicked(object sender, EventArgs e)
        {
            double income = double.TryParse(IncomeEntry.Text, out var i) ? i : 0;
            double expenditure = double.TryParse(ExpenditureEntry.Text, out var ex) ? ex : 0;
            double net = income - expenditure;

            NetIncomeLabel.Text = net.ToString("C");
        }
    }
}