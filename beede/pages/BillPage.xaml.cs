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
        // 获取所有账单
        var allBills = BillService.Bills;

        // 收入账单（按日期倒序）
        var incomes = allBills.Where(b => b.IsIncome).OrderByDescending(b => b.Date).ToList();
        IncomeListView.ItemsSource = incomes;

        // 支出账单（按日期倒序）
        var expenses = allBills.Where(b => !b.IsIncome).OrderByDescending(b => b.Date).ToList();
        ExpenseListView.ItemsSource = expenses;
    }

    private void UpdateNetIncome()
    {
        NetIncomeLabel.Text = BillService.SavedAmount.ToString("C");
    }

    // 点击账单时弹出删除确认
    private async void OnBillSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Bill selectedBill)
        {
            // 清除选中状态
            if (sender is CollectionView cv)
                cv.SelectedItem = null;

            // 确认删除
            bool confirm = await DisplayAlertAsync("删除账单",
                $"确定要删除 \"{selectedBill.Description}\" 吗？", "删除", "取消");

            if (confirm)
            {
                BillService.RemoveBill(selectedBill);

                // 刷新列表
                RefreshBillLists();
                UpdateNetIncome();

                // 显示通知
                await DisplayAlertAsync("通知", "账单已删除", "OK");
            }
        }
    }
}