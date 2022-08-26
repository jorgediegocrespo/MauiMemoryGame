namespace MauiMemoryGame.Base;

public class BaseContentView : ContentView
{
    public BaseContentView()
    {
    }

    public virtual void HandleActivation(CompositeDisposable disposables)
    {
        CreateBindings(disposables);
        ObserveValues(disposables);
        CreateEvents(disposables);
    }

    public virtual void HandleDeactivation()
    { }

    protected virtual void CreateBindings(CompositeDisposable disposables)
    { }

    protected virtual void ObserveValues(CompositeDisposable disposables)
    { }

    protected virtual void CreateEvents(CompositeDisposable disposables)
    { }
}
