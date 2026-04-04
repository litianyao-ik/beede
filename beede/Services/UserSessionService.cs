using System.Text.Json;
using beede.Services;

namespace beede.Services;

public class UserSessionService
{
    private const string UsersFileName = "users.json";
    private string UsersFilePath => Path.Combine(FileSystem.AppDataDirectory, UsersFileName);

    public string? CurrentUser { get; private set; }

    // 加载所有用户列表
    public List<string> LoadAllUsers()
    {
        if (!File.Exists(UsersFilePath))
            return new List<string>();  // 保持原样

        var json = File.ReadAllText(UsersFilePath);
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }

    // 保存用户列表
    private void SaveAllUsers(List<string> users)
    {
        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(UsersFilePath, json);
    }

    // 检查用户是否存在
    public bool UserExists(string username)
    {
        var users = LoadAllUsers();
        return users.Contains(username);
    }

    // 注册新用户
    public bool RegisterUser(string username, string password)
    {
        var users = LoadAllUsers();

        if (users.Contains(username))
            return false;

        // 保存密码
        var passwordHash = SimpleHash(password);
        var pwdFile = Path.Combine(FileSystem.AppDataDirectory, $"user_{username}.pwd");
        File.WriteAllText(pwdFile, passwordHash);

        // 添加到用户列表
        users.Add(username);
        SaveAllUsers(users);

        // 创建空的账单文件
        var billsFile = Path.Combine(FileSystem.AppDataDirectory, $"bills_{username}.json");
        File.WriteAllText(billsFile, "[]");

        return true;
    }

    // 登录
    public bool Login(string username, string password)
    {
        var pwdFile = Path.Combine(FileSystem.AppDataDirectory, $"user_{username}.pwd");
        if (!File.Exists(pwdFile))
            return false;

        var storedHash = File.ReadAllText(pwdFile);
        if (SimpleHash(password) != storedHash)
            return false;

        CurrentUser = username;

        // 加载该用户的账单数据
        BillService.LoadUserBills(username);

        // 发送登录通知到 API
        _ = DemoApiService.SendLoginNotification(username);

        return true;
    }

    // 登出
    public void Logout()
    {
        CurrentUser = null;
        BillService.ClearAllBills();
    }

    // 简单的哈希函数
    private static string SimpleHash(string input)  // 添加 static
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}