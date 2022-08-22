namespace MauiMemoryGame.Services;

public class DialogService : IDialogService
{
    public async Task ShowDialogAsync(string title, string message, string close)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, close);
    }

    public async Task<bool> ShowDialogConfirmationAsync(string title, string message, string cancel, string ok)
    {
        return await Application.Current.MainPage.DisplayAlert(title, message, ok, cancel);
    }

    public async Task<string> DisplayActionSheet(string title, string cancel, string[] buttons)
    {
        return await Application.Current.MainPage.DisplayActionSheet(title, cancel, null, buttons);
    }
}

