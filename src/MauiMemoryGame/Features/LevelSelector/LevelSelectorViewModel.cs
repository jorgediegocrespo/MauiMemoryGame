namespace MauiMemoryGame.Features;

[QueryProperty(nameof(SelectedTheme), nameof(SelectedTheme))]
public class LevelSelectorViewModel : BaseViewModel
{
    public LevelSelectorViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    [Reactive] public Themes SelectedTheme { get; private set; }

    public ReactiveCommand<Unit, Unit> SelectHighCommand { get; private set; }
    public extern bool IsSelectingHigh { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectMediumCommand { get; private set; }
    public extern bool IsSelectingMedium { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectLowCommand { get; private set; }
    public extern bool IsSelectingLow { [ObservableAsProperty] get; }

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);

        SelectHighCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectHighCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingHigh).DisposeWith(disposables);

        SelectMediumCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectMediumCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingMedium).DisposeWith(disposables);

        SelectLowCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectLowCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingLow).DisposeWith(disposables);
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        SelectHighCommand = ReactiveCommand.CreateFromTask(SelectHighAsync);
        SelectMediumCommand = ReactiveCommand.CreateFromTask(SelectMediumAsync);
        SelectLowCommand = ReactiveCommand.CreateFromTask(SelectLowAsync);
    }

    private Task SelectHighAsync() => navigationService.NavigateToGame(SelectedTheme, Level.High);

    private Task SelectMediumAsync() => navigationService.NavigateToGame(SelectedTheme, Level.Medium);

    private Task SelectLowAsync() => navigationService.NavigateToGame(SelectedTheme, Level.Low);
}
