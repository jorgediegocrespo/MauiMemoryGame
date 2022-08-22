namespace MauiMemoryGame.Base;

public abstract class BaseViewModel : ReactiveObject
{
    protected CompositeDisposable disposables;
    protected readonly ILogService logService;
    protected readonly INavigationService navigationService;

    public BaseViewModel(ILogService logService, INavigationService navigationService)
    {
        this.logService = logService;
        this.navigationService = navigationService;

        CreateCommands();
    }

    public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; private set; }
    public extern bool IsNavigatingBack { [ObservableAsProperty] get; }

    public virtual Task OnAppearingAsync()
    {
        disposables = disposables ?? new CompositeDisposable();
        disposables.Add(NavigateBackCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(NavigateBackCommand.IsExecuting.ToPropertyEx(this, x => x.IsNavigatingBack));

        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync()
    {
        disposables?.Dispose();
        disposables = null;
        return Task.CompletedTask;
    }

    protected virtual void CreateCommands()
    {
        NavigateBackCommand = ReactiveCommand.CreateFromTask(NavigateBackAsync);
    }

    protected virtual Task NavigateBackAsync()
    {
        return navigationService.NavigateBack();
    }
}