namespace beede.Models
{
    public class Bill
    {
        public string Description { get; set; }   // 描述
        public double Amount { get; set; }        // 金额
        public DateTime Date { get; set; }         // 日期
        public bool IsIncome { get; set; }         // true=收入，false=支出
    }
}