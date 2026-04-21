using System.Text.Json;
using beede.Services;

namespace beede.Services;

public class UserSessionService
{
    private const string UsersFileName = "users.json";
    private string UsersFilePath => Path.Combine(FileSystem.AppDataDirectory, UsersFileName);

    public string? CurrentUser { get; private set; }

    // Load all user list
    public List<string> LoadAllUsers()
    {
        if (!File.Exists(UsersFilePath))
            return new List<string>();

        var json = File.ReadAllText(UsersFilePath);
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }

    // Save user list
    private void SaveAllUsers(List<string> users)
    {
        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(UsersFilePath, json);
    }

    // Check if user exists
    public bool UserExists(string username)
    {
        var users = LoadAllUsers();
        return users.Contains(username);
    }

    // Register new user
    public bool RegisterUser(string username, string password)
    {
        var users = LoadAllUsers();

        if (users.Contains(username))
            return false;

        // Save password
        var passwordHash = SimpleHash(password);
        var pwdFile = Path.Combine(FileSystem.AppDataDirectory, $"user_{username}.pwd");
        File.WriteAllText(pwdFile, passwordHash);

        // Add to user list
        users.Add(username);
        SaveAllUsers(users);

        // Create empty bill file
        var billsFile = Path.Combine(FileSystem.AppDataDirectory, $"bills_{username}.json");
        File.WriteAllText(billsFile, "[]");

        return true;
    }

    // Login
    public bool Login(string username, string password)
    {
        var pwdFile = Path.Combine(FileSystem.AppDataDirectory, $"user_{username}.pwd");
        if (!File.Exists(pwdFile))
            return false;

        var storedHash = File.ReadAllText(pwdFile);
        if (SimpleHash(password) != storedHash)
            return false;

        CurrentUser = username;

        // Load the user's bill data
        BillService.LoadUserBills(username);

        // Send login notification to API
        _ = DemoApiService.SendLoginNotification(username);

        return true;
    }

    // Logout
    public void Logout()
    {
        CurrentUser = null;
        BillService.ClearAllBills();
    }

    // Simple hash function
    private static string SimpleHash(string input)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}