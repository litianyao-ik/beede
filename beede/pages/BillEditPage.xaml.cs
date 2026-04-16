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
        // 验证描述
        if (DescriptionEntry == null || string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            await DisplayAlertAsync("提示", "请输入描述", "OK");
            return;
        }

        // 验证金额
        if (AmountEntry == null || !double.TryParse(AmountEntry.Text, out double amount))
        {
            await DisplayAlertAsync("提示", "请输入有效的金额", "OK");
            return;
        }

        // 创建账单
        var bill = new Bill
        {
            Description = DescriptionEntry.Text.Trim(),
            Amount = amount,
            IsIncome = TypePicker?.SelectedIndex == 0,
            Date = DateTime.Now
        };

        // 保存账单
        BillService.AddBill(bill);

        // 显示通知
        string typeText = bill.IsIncome ? "收入" : "支出";
        await DisplayAlertAsync("完成一笔新的记账",
            $"已添加 {typeText}：{bill.Description}，金额 {bill.Amount:C}", "OK");

        // 返回上一页
        await Navigation.PopAsync();
    }
}