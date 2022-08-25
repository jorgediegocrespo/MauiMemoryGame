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

        return GoToAnimated(nameof(LevelSelectorView), navigationParameter);
    }

    public Task NavigateToGame(Themes selectedTheme, Level selectedLevel)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(GameViewModel.SelectedTheme), selectedTheme },
            { nameof(GameViewModel.SelectedLevel), selectedLevel }
        };

        return GoToAnimated(nameof(GameView), navigationParameter);
    }

    public Task NavigateToGameOverPopup(Themes selectedTheme, Level selectedLevel, bool isWinner)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(GameOverPopupViewModel.SelectedTheme), selectedTheme },
            { nameof(GameOverPopupViewModel.SelectedLevel), selectedLevel },
            { nameof(GameOverPopupViewModel.IsWinner), isWinner }
        };

        return GoToAnimated(nameof(GameOverPopupView), navigationParameter);
    }

    public async Task PlayAgainFromGameOver(Themes selectedTheme, Level selectedLevel)
    {
        var navigationParameter = new Dictionary<string, object>
        {
            { nameof(GameViewModel.SelectedTheme), selectedTheme },
            { nameof(GameViewModel.SelectedLevel), selectedLevel }
        };

        await GoToAnimated($"../{nameof(GameView)}", navigationParameter);
        RemovePreviousPage();
    }

    public Task NavigateBack()
    {
        return GoToAnimated("..");
    }

    public async Task NavigateBackToStart()
    {
        await ((IAnimatedPage)Shell.Current.CurrentPage).RunDisappearingAnimationAsync();
        await Shell.Current.Navigation.PopToRootAsync(false);
    }

    private async Task GoToAnimated(ShellNavigationState state, IDictionary<string, object> parameters)
    {
        await ((IAnimatedPage)Shell.Current.CurrentPage).RunDisappearingAnimationAsync();
        await Shell.Current.GoToAsync(state, false, parameters);
    }

    private async Task GoToAnimated(ShellNavigationState state)
    {
        await ((IAnimatedPage)Shell.Current.CurrentPage).RunDisappearingAnimationAsync();
        await Shell.Current.GoToAsync(state, false);
    }

    private void RemovePreviousPage()
    {
        Page pageToRemove = Shell.Current.Navigation.NavigationStack[Shell.Current.Navigation.NavigationStack.Count - 2];
        Shell.Current.Navigation.RemovePage(pageToRemove);
    }
}
