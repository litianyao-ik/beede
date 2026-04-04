using beede.Models;
using EmbedIO;
using EmbedIO.WebApi;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace beede.Services;

public class BillsController : WebApiController
{
    [Route(HttpVerbs.Get, "/api/bills")]
    public object GetAllBills()
    {
        return BillService.Bills;
    }

    [Route(HttpVerbs.Get, "/api/bills/summary")]
    public object GetSummary()
    {
        return new
        {
            totalBills = BillService.TotalBills,
            totalIncome = BillService.TotalIncome,
            totalExpenditure = BillService.TotalExpenditure,
            savedAmount = BillService.SavedAmount
        };
    }

    [Route(HttpVerbs.Get, "/api/bills/income")]
    public object GetIncome()
    {
        return BillService.Bills.Where(b => b.IsIncome);
    }

    [Route(HttpVerbs.Get, "/api/bills/expense")]
    public object GetExpense()
    {
        return BillService.Bills.Where(b => !b.IsIncome);
    }

    [Route(HttpVerbs.Post, "/api/bills")]
    public async Task<object> AddBill()
    {
        var requestBody = await HttpContext.GetRequestBodyAsStringAsync();
        var bill = JsonSerializer.Deserialize<Bill>(requestBody);

        if (bill == null)
            throw new HttpException(400, "Invalid bill data");

        bill.Date = DateTime.Now;
        BillService.AddBill(bill);

        HttpContext.Response.StatusCode = 201;
        return bill;
    }

    [Route(HttpVerbs.Delete, "/api/bills/all")]
    public object ClearAllBills()
    {
        BillService.ClearAllBills();
        return new { message = "All bills cleared" };
    }
}

public static class ApiService
{
    private static WebServer? _server;
    private static UserSessionService? _session;

    public static void StartApiServer(UserSessionService session)
    {
        _session = session;

        _server = new WebServer(o => o
                .WithUrlPrefix("http://localhost:5000")
                .WithMode(HttpListenerMode.EmbedIO))
            .WithWebApi("/", m => m.WithController<BillsController>());

        _server.OnHttpException = async (context, exception) =>
        {
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsync(exception.Message);
        };

        _server.RunAsync();
    }

    public static void StopApiServer()
    {
        _server?.Dispose();
    }
}