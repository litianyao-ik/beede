using System.Net;
using System.Text;

namespace beede.Services;

public static class DemoApiService
{
    private static HttpListener? _listener;
    private static bool _isRunning;

    public static void StartServer()
    {
        if (_isRunning) return;

        Task.Run(async () =>
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:5000/");

            try
            {
                _listener.Start();
                _isRunning = true;
                System.Diagnostics.Debug.WriteLine("[API] Server started: http://localhost:5000");

                while (_isRunning)
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => ProcessRequest(context));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[API] Failed to start: {ex.Message}");
            }
        });
    }

    public static void StopServer()
    {
        _isRunning = false;
        _listener?.Stop();
        _listener?.Close();
        System.Diagnostics.Debug.WriteLine("[API] Server stopped");
    }

    public static async Task SendLoginNotification(string username)
    {
        if (!_isRunning) return;

        try
        {
            var message = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] User '{username}' logged into the application";
            System.Diagnostics.Debug.WriteLine(message);

            using var client = new HttpClient();
            var content = new StringContent(
                $"{{\"username\":\"{username}\",\"message\":\"User login\",\"timestamp\":\"{DateTime.Now:O}\"}}",
                Encoding.UTF8,
                "application/json");

            try
            {
                await client.PostAsync("https://your-server.com/api/login", content);
            }
            catch
            {
                // Silently ignore if external API does not exist
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[API] Failed to send notification: {ex.Message}");
        }
    }

    private static async Task ProcessRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (request.Url?.AbsolutePath == "/api/login" && request.HttpMethod == "POST")
        {
            string body;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }

            System.Diagnostics.Debug.WriteLine($"[API] Received login request: {body}");

            var responseText = "{\"status\":\"ok\",\"message\":\"Login notification received\"}";
            var buffer = Encoding.UTF8.GetBytes(responseText);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.StatusCode = 200;
        }
        else
        {
            response.StatusCode = 404;
            var buffer = Encoding.UTF8.GetBytes("{\"error\":\"Not Found\"}");
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        response.OutputStream.Close();
    }
}