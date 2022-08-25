namespace MauiMemoryGame.Features;

public class GameOverPopupViewModel : BaseViewModel, IQueryAttributable
{
    public GameOverPopupViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    public Themes SelectedTheme { get; private set; }
    public Level SelectedLevel { get; private set; }
    [Reactive] public bool IsWinner { get; private set; }

    public ReactiveCommand<Unit, Unit> PlayAgainCommand { get; private set; }
    public extern bool IsGoingToPlayAgain { [ObservableAsProperty] get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedTheme = (Themes)query[nameof(SelectedTheme)];
        SelectedLevel = (Level)query[nameof(SelectedLevel)];
        IsWinner = (bool)query[nameof(IsWinner)];
    }

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);

        PlayAgainCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        PlayAgainCommand.IsExecuting.ToPropertyEx(this, x => x.IsGoingToPlayAgain).DisposeWith(disposables);
    }

    protected override Task NavigateBackAsync()
    {
        return navigationService.NavigateBackToStart();
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        PlayAgainCommand = ReactiveCommand.CreateFromTask(PlayAgainAsync);
    }

    private Task PlayAgainAsync()
    {
        return navigationService.PlayAgainFromGameOver(SelectedTheme, SelectedLevel);
    }
}
