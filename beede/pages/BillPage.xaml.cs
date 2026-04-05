using Microsoft.Maui.Controls;

namespace beede.Pages;

public partial class BillPage : ContentPage
{
    public BillPage()
    {
        InitializeComponent();
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        double income = 0;
        double expenditure = 0;

        if (IncomeEntry != null && double.TryParse(IncomeEntry.Text, out var i))
            income = i;

        if (ExpenditureEntry != null && double.TryParse(ExpenditureEntry.Text, out var ex))
            expenditure = ex;

        double net = income - expenditure;

        if (NetIncomeLabel != null)
            NetIncomeLabel.Text = net.ToString("C");
    }
}