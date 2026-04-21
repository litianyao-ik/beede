namespace beede.Models
{
    public class Bill
    {
        public string Description { get; set; } = string.Empty;  // Added = string.Empty
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsIncome { get; set; }
    }
}