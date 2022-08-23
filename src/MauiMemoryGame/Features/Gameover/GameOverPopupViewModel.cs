namespace MauiMemoryGame.Features;

[QueryProperty(nameof(IsWinner), nameof(IsWinner))]
public class GameOverPopupViewModel : BaseViewModel
{
    public GameOverPopupViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    [Reactive] public bool IsWinner { get; set; }

    public ReactiveCommand<Unit, Unit> PlayAgainCommand { get; private set; }
    public extern bool IsGoingToPlayAgain { [ObservableAsProperty] get; }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();

        disposables.Add(PlayAgainCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(PlayAgainCommand.IsExecuting.ToPropertyEx(this, x => x.IsGoingToPlayAgain));
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        PlayAgainCommand = ReactiveCommand.CreateFromTask(navigationService.NavigateBack);
    }

    protected override Task NavigateBackAsync()
    {
        return navigationService.NavigateBackToStart();
    }
}
