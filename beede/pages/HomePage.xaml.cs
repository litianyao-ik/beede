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

        // 搜索匹配的账单
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

        bool confirm = await DisplayAlertAsync("删除账单",
            $"确定要删除 \"{bill.Description}\" 吗？", "删除", "取消");

        if (confirm)
        {
            BillService.RemoveBill(bill);
            RefreshStats();

            // 刷新搜索结果
            OnSearchTextChanged(SearchBar, new TextChangedEventArgs(SearchBar.Text, SearchBar.Text));

            await DisplayAlertAsync("通知", "账单已删除", "OK");
        }
    }

    private async void OnNewClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillEditPage());
    }
}