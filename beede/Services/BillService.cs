using beede.Models;

namespace beede.Services;

public static class BillService
{
    private static readonly List<Bill> _bills = new();
    private static string _currentUser = string.Empty;
    private static string DataFolder => FileSystem.AppDataDirectory;

    // 公开的账单列表（只读）
    public static IReadOnlyList<Bill> Bills => _bills.AsReadOnly();

    // 统计数据
    public static int TotalBills => _bills.Count;
    public static double TotalIncome => _bills.Where(b => b.IsIncome).Sum(b => b.Amount);
    public static double TotalExpenditure => _bills.Where(b => !b.IsIncome).Sum(b => b.Amount);
    public static double SavedAmount => TotalIncome - TotalExpenditure;

    // 获取当前用户的账单文件路径
    private static string GetBillsFilePath(string username) =>
        Path.Combine(DataFolder, $"bills_{username}.json");

    // 加载指定用户的账单
    public static bool LoadUserBills(string username)
    {
        _currentUser = username;
        var filePath = GetBillsFilePath(username);

        if (!File.Exists(filePath))
        {
            _bills.Clear();
            return true;
        }

        try
        {
            var json = File.ReadAllText(filePath);
            var bills = System.Text.Json.JsonSerializer.Deserialize<List<Bill>>(json);
            _bills.Clear();
            if (bills != null)
                _bills.AddRange(bills);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // 保存当前用户的账单
    private static void SaveCurrentUserBills()
    {
        if (string.IsNullOrEmpty(_currentUser)) return;

        var filePath = GetBillsFilePath(_currentUser);
        var json = System.Text.Json.JsonSerializer.Serialize(_bills);
        File.WriteAllText(filePath, json);
    }

    // 添加账单
    public static void AddBill(Bill bill)
    {
        _bills.Add(bill);
        SaveCurrentUserBills();
    }

    // 删除账单
    public static bool RemoveBill(Bill bill)
    {
        var result = _bills.Remove(bill);
        if (result) SaveCurrentUserBills();
        return result;
    }

    // 清空所有账单
    public static void ClearAllBills()
    {
        _bills.Clear();
        SaveCurrentUserBills();
    }
}