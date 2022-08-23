namespace MauiMemoryGame;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(LevelSelectorView), typeof(LevelSelectorView));
        Routing.RegisterRoute(nameof(GameView), typeof(GameView));
        Routing.RegisterRoute(nameof(GameOverPopupView), typeof(GameOverPopupView));
    }
}