namespace MauiMemoryGame.Services;

public interface IDialogService
{
    Task ShowDialogAsync(string title, string message, string close);
    Task<bool> ShowDialogConfirmationAsync(string title, string message, string cancel, string ok);
    Task<string> DisplayActionSheet(string title, string cancel, string[] buttons);
}

