using beede.Services;  // 引入 BillService

namespace beede.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    // 每次页面出现时刷新统计数据（因为从新增页面返回时会触发）
    protected override void OnAppearing()
    {
        base.OnAppearing();
        RecordedBillsLabel.Text = BillService.TotalBills.ToString();
        SavedAmountLabel.Text = BillService.SavedAmount.ToString("C"); // "C" 格式化为货币
    }

    // 点击 NEW 按钮时跳转到新增账单页面
    private async void OnNewClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BillEditPage());
    }
}