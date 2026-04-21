using beede.Models;
using beede.Services;
using Microsoft.Maui.Controls;

namespace beede.Pages;

public partial class BillEditPage : ContentPage
{
    public BillEditPage()
    {
        InitializeComponent();

        if (TypePicker != null)
            TypePicker.SelectedIndex = 0;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        // Validate description
        if (DescriptionEntry == null || string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            await DisplayAlertAsync("Notice", "Please enter a description", "OK");
            return;
        }

        // Validate amount
        if (AmountEntry == null || !double.TryParse(AmountEntry.Text, out double amount))
        {
            await DisplayAlertAsync("Notice", "Please enter a valid amount", "OK");
            return;
        }

        // Create bill
        var bill = new Bill
        {
            Description = DescriptionEntry.Text.Trim(),
            Amount = amount,
            IsIncome = TypePicker?.SelectedIndex == 0,
            Date = DateTime.Now
        };

        // Save bill
        BillService.AddBill(bill);

        // Show notification
        string typeText = bill.IsIncome ? "Income" : "Expense";
        await DisplayAlertAsync("New Record Added",
            $"Added {typeText}: {bill.Description}, Amount: {bill.Amount:C}", "OK");

        // Go back to previous page
        await Navigation.PopAsync();
    }
}