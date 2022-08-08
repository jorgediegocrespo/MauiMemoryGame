namespace MauiMemoryGame.Services;

public class NavigationService : INavigationService
{
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

    public Task NavigateBack()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task NavigateBackToStart()
    {
        return Shell.Current.GoToAsync("../themeselector");
    }
}
