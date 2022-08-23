namespace MauiMemoryGame.Services;

public interface INavigationService
{
    Task NavigateToLevelSelection(Themes selectedTheme);
    Task NavigateToGame(Themes selectedTheme, Level selectedLevel);
    Task NavigateToGameOverPopup(bool isWinner);
    Task NavigateBack();
    Task NavigateBackToStart();
}
