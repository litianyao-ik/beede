using beede.Models;
using beede.Services;
using Microsoft.Maui.Controls;
using System;

namespace beede.Pages
{
    public partial class BillEditPage : ContentPage
    {
        public BillEditPage()
        {
            InitializeComponent();
            TypePicker.SelectedIndex = 0;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
            {
                await DisplayAlert("提示", "请输入描述", "OK");
                return;
            }

            if (!double.TryParse(AmountEntry.Text, out double amount))
            {
                await DisplayAlert("提示", "请输入有效的金额", "OK");
                return;
            }

            var bill = new Bill
            {
                Description = DescriptionEntry.Text,
                Amount = amount,
                IsIncome = TypePicker.SelectedIndex == 0,
                Date = DateTime.Now
            };

            BillService.Bills.Add(bill);
            await Navigation.PopAsync();
        }
    }
}