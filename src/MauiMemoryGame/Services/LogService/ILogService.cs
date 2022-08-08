namespace MauiMemoryGame.Services;

public interface ILogService
{
    void TraceEvent(string message);
    void TraceError(Exception ex);
}
