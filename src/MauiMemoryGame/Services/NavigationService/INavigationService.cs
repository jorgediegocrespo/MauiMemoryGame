namespace MauiMemoryGame.Services;

public interface INavigationService
{
    Task NavigateToLevelSelection(Themes selectedTheme);
    Task NavigateToGame(Themes selectedTheme, Level selectedLevel);
    Task NavigateToGameOverPopup(Themes selectedTheme, Level selectedLevel, bool isWinner);
    Task PlayAgainFromGameOver(Themes selectedTheme, Level selectedLevel);
    Task NavigateBack();
    Task NavigateBackToStart();
}
