using beede.Models; // 引用 Bill 类

namespace beede.Services;  // 或者 using beede.Services; 根据你的文件夹命名

public static class BillService
{
    // 存储所有账单的列表
    public static List<Bill> Bills { get; } = new List<Bill>();

    // 总账单数
    public static int TotalBills => Bills.Count;

    // 总收入
    public static double TotalIncome => Bills.Where(b => b.IsIncome).Sum(b => b.Amount);

    // 总支出
    public static double TotalExpenditure => Bills.Where(b => !b.IsIncome).Sum(b => b.Amount);

    // 净节省（收入 - 支出）
    public static double SavedAmount => TotalIncome - TotalExpenditure;
}