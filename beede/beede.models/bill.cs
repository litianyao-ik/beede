namespace beede.Models
{
    public class Bill
    {
        public string Description { get; set; } = string.Empty;  // 添加 = string.Empty
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsIncome { get; set; }
    }
}