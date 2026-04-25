using beede.Models;
using beede.Services;
using Microsoft.Maui.Controls;

namespace beede.Pages;

public partial class BillPage : ContentPage
{
    public BillPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        RefreshBillLists();
        UpdateNetIncome();
    }

    private void RefreshBillLists()
    {
        // Get all bills
        var allBills = BillService.Bills;

        // Income bills (sorted by date descending)
        var incomes = allBills.Where(b => b.IsIncome).OrderByDescending(b => b.Date).ToList();
        IncomeListView.ItemsSource = incomes;

        // Expense bills (sorted by date descending)
        var expenses = allBills.Where(b => !b.IsIncome).OrderByDescending(b => b.Date).ToList();
        ExpenseListView.ItemsSource = expenses;
    }

    private void UpdateNetIncome()
    {
        NetIncomeLabel.Text = BillService.SavedAmount.ToString("C");
    }

    // Show delete confirmation when a bill is tapped
    private async void OnBillSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Bill selectedBill)
        {
            // Clear selection state
            if (sender is CollectionView cv)
                cv.SelectedItem = null;

            // Confirm deletion
            bool confirm = await DisplayAlertAsync("Delete Bill",
                $"Are you sure you want to delete \"{selectedBill.Description}\"?", "Delete", "Cancel");

            if (confirm)
            {
                BillService.RemoveBill(selectedBill);

                // Refresh lists
                RefreshBillLists();
                UpdateNetIncome();

                // Show notification
                await DisplayAlertAsync("Notice", "Bill has been deleted", "OK");
            }
        }
    }
}