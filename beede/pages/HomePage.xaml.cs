using beede.Services;
using Microsoft.Maui.Controls;

namespace beede.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshData();
    }

    private void RefreshData()
    {
        RecordedBillsLabel.Text = BillService.TotalBills.ToString();
        SavedAmountLabel.Text = BillService.SavedAmount.ToString("C");
    }

    private async void OnNewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillEditPage());
    }
}