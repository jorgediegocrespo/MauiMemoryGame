namespace MauiMemoryGame.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Task NavigateToLevelSelection(Themes selectedTheme)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(LevelSelectorViewModel.SelectedTheme), selectedTheme }
        };

        return Shell.Current.GoToAsync(nameof(LevelSelectorView), true, navigationParameter);
    }

    public Task NavigateToGame(Themes selectedTheme, Level selectedLevel)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(GameViewModel.SelectedTheme), selectedTheme },
            { nameof(GameViewModel.SelectedLevel), selectedLevel }
        };

        return Shell.Current.GoToAsync(nameof(GameView), navigationParameter);
    }

    public Task NavigateToGameOverPopup(bool isWinner)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(GameOverPopupViewModel.IsWinner), isWinner }
        };

        return Shell.Current.GoToAsync(nameof(GameOverPopupView), navigationParameter);
    }

    public Task NavigateBack()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task NavigateBackToStart()
    {
        return Shell.Current.Navigation.PopToRootAsync();
    }
}
