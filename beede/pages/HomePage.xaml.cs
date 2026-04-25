using beede.Models;
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
        RefreshStats();
        ClearSearch();
    }

    private void RefreshStats()
    {
        RecordedBillsLabel.Text = BillService.TotalBills.ToString();
        SavedAmountLabel.Text = BillService.SavedAmount.ToString("C");
    }

    private void ClearSearch()
    {
        SearchBar.Text = string.Empty;
        SearchResultsView.ItemsSource = null;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var keyword = e.NewTextValue?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(keyword))
        {
            SearchResultsView.ItemsSource = null;
            return;
        }

        // Search for matching bills
        var results = BillService.Bills
            .Where(b => b.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(b => b.Date)
            .ToList();

        SearchResultsView.ItemsSource = results;
    }

    private async void OnDeleteSearchResult(object sender, EventArgs e)
    {
        var button = sender as Button;
        var bill = button?.CommandParameter as Bill;

        if (bill == null) return;

        bool confirm = await DisplayAlertAsync("Delete Bill",
            $"Are you sure you want to delete \"{bill.Description}\"?", "Delete", "Cancel");

        if (confirm)
        {
            BillService.RemoveBill(bill);
            RefreshStats();

            // Refresh search results
            OnSearchTextChanged(SearchBar, new TextChangedEventArgs(SearchBar.Text, SearchBar.Text));

            await DisplayAlertAsync("Notice", "Bill has been deleted", "OK");
        }
    }

    private async void OnNewClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillEditPage());
    }
}