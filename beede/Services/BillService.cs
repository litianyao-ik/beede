using beede.Models;

namespace beede.Services;

public static class BillService
{
    private static readonly List<Bill> _bills = new();
    private static string _currentUser = string.Empty;
    private static string DataFolder => FileSystem.AppDataDirectory;

    // Public bill list (read-only)
    public static IReadOnlyList<Bill> Bills => _bills.AsReadOnly();

    // Statistics
    public static int TotalBills => _bills.Count;
    public static double TotalIncome => _bills.Where(b => b.IsIncome).Sum(b => b.Amount);
    public static double TotalExpenditure => _bills.Where(b => !b.IsIncome).Sum(b => b.Amount);
    public static double SavedAmount => TotalIncome - TotalExpenditure;

    // Get the bill file path for the current user
    private static string GetBillsFilePath(string username) =>
        Path.Combine(DataFolder, $"bills_{username}.json");

    // Load bills for a specific user
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

    // Save the current user's bills
    private static void SaveCurrentUserBills()
    {
        if (string.IsNullOrEmpty(_currentUser)) return;

        var filePath = GetBillsFilePath(_currentUser);
        var json = System.Text.Json.JsonSerializer.Serialize(_bills);
        File.WriteAllText(filePath, json);
    }

    // Add a bill
    public static void AddBill(Bill bill)
    {
        _bills.Add(bill);
        SaveCurrentUserBills();
    }

    // Delete a bill
    public static bool RemoveBill(Bill bill)
    {
        var result = _bills.Remove(bill);
        if (result) SaveCurrentUserBills();
        return result;
    }

    // Clear all bills
    public static void ClearAllBills()
    {
        _bills.Clear();
        SaveCurrentUserBills();
    }
}