using System.Reactive.Concurrency;

namespace MauiMemoryGame.Features;

public class ThemeSelectorViewModel : BaseViewModel
{
    public ThemeSelectorViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    public ReactiveCommand<Unit, Unit> SelectDcCommand { get; private set; }
    public extern bool IsSelectingDc { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectMarvelCommand { get; private set; }
    public extern bool IsSelectingMarvel { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectSimpsonsCommand { get; private set; }
    public extern bool IsSelectingSimpsons { [ObservableAsProperty] get; }

    public ReactiveCommand<Unit, Unit> SelectStarWarsCommand { get; private set; }
    public extern bool IsSelectingStarWars { [ObservableAsProperty] get; }

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);

        SelectDcCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectDcCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingDc).DisposeWith(disposables);

        SelectMarvelCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectMarvelCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingMarvel).DisposeWith(disposables);

        SelectSimpsonsCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectSimpsonsCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingSimpsons).DisposeWith(disposables);

        SelectStarWarsCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        SelectStarWarsCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingStarWars).DisposeWith(disposables);
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        SelectDcCommand = ReactiveCommand.CreateFromTask(SelectDcAsync);
        SelectMarvelCommand = ReactiveCommand.CreateFromTask(SelectMarvelAsync);
        SelectSimpsonsCommand = ReactiveCommand.CreateFromTask(SelectSimpsonsAsync);
        SelectStarWarsCommand = ReactiveCommand.CreateFromTask(SelectStarWarsAsync);
    }

    private Task SelectDcAsync() => navigationService.NavigateToLevelSelection(Themes.DC);

    private Task SelectMarvelAsync() => navigationService.NavigateToLevelSelection(Themes.Marvel);

    private Task SelectSimpsonsAsync() => navigationService.NavigateToLevelSelection(Themes.Simpsons);

    private Task SelectStarWarsAsync() => navigationService.NavigateToLevelSelection(Themes.StarWars);
}