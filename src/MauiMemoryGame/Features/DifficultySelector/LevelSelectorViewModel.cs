﻿namespace MauiMemoryGame.Features;

[QueryProperty(nameof(SelectedTheme), nameof(SelectedTheme))]
public class LevelSelectorViewModel : BaseViewModel
{
    public LevelSelectorViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    [Reactive] public Themes SelectedTheme { get; set; }

    public ReactiveCommand<Unit, Unit> SelectHighCommand { get; private set; }
    public extern bool IsSelectingHigh { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectMediumCommand { get; private set; }
    public extern bool IsSelectingMedium { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectLowCommand { get; private set; }
    public extern bool IsSelectingLow { [ObservableAsProperty] get; }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();

        disposables.Add(SelectHighCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectHighCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingHigh));

        disposables.Add(SelectMediumCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectMediumCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingMedium));

        disposables.Add(SelectLowCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectLowCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingLow));
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
