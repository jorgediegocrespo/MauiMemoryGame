using System.Diagnostics;

namespace MauiMemoryGame.Services;

public class LogService : ILogService
{
    public void TraceError(Exception ex)
    {
        Debug.WriteLine($"Exception => {ex?.Message}{Environment.NewLine}    {ex?.StackTrace}");
    }

    public void TraceEvent(string message)
    {
        Debug.WriteLine($"Event => {message}{Environment.NewLine}");
    }
}
