namespace MauiMemoryGame.Base;

public abstract class BaseViewModel : ReactiveObject, IActivatableViewModel
{
    protected readonly ILogService logService;
    protected readonly INavigationService navigationService;

    public BaseViewModel(ILogService logService, INavigationService navigationService)
    {
        this.logService = logService;
        this.navigationService = navigationService;
        
        CreateCommands();

        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            HandleActivation(disposables);

            Disposable
                .Create(() => HandleDeactivation())
                .DisposeWith(disposables);
        });
    }

    public ViewModelActivator Activator { get; }

    public ReactiveCommand<Unit, Unit> NavigateBackCommand { get; private set; }
    public extern bool IsNavigatingBack { [ObservableAsProperty] get; }

    public virtual Task OnAppearingAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task OnDisappearingAsync()
    {
        return Task.CompletedTask;
    }
    protected virtual void HandleActivation(CompositeDisposable disposables)
    {
        NavigateBackCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        NavigateBackCommand.IsExecuting.ToPropertyEx(this, x => x.IsNavigatingBack).DisposeWith(disposables);
    }

    protected virtual void HandleDeactivation()
    { }

    protected virtual void CreateCommands()
    {
        NavigateBackCommand = ReactiveCommand.CreateFromTask(NavigateBackAsync);
    }

    protected virtual Task NavigateBackAsync() => navigationService.NavigateBack();
}