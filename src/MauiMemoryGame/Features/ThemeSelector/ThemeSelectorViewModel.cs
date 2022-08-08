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

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();

        disposables.Add(SelectDcCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectDcCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingDc));

        disposables.Add(SelectMarvelCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectMarvelCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingMarvel));

        disposables.Add(SelectSimpsonsCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectSimpsonsCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingSimpsons));

        disposables.Add(SelectStarWarsCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(SelectStarWarsCommand.IsExecuting.ToPropertyEx(this, x => x.IsSelectingStarWars));
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