namespace MauiMemoryGame.Services;

public interface INavigationService
{
    Task NavigateToLevelSelection(Themes selectedTheme);
    Task NavigateToGame(Themes selectedTheme, Level selectedLevel);
    Task NavigateBackToGameOver(bool gameWon);
    Task NavigateBack();
    Task NavigateBackToStart();
}
