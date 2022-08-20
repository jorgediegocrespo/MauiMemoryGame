namespace MauiMemoryGame.Services;

public interface INavigationService
{
    Task NavigateToLevelSelection(Themes selectedTheme);
    Task NavigateToGame(Themes selectedTheme, Level selectedLevel);
    Task<bool> NavigateBackToGameOver(bool isWinner);
    Task NavigateBack();
    Task NavigateBackToStart();
}
