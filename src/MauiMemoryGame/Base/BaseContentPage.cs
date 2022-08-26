using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using ReactiveUI.Maui;

namespace MauiMemoryGame.Base;

public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel>, IAnimatedPage
    where TViewModel : BaseViewModel
{
    private bool appearingAnimationDone;

    public BaseContentPage()
    {
        On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        this.WhenActivated(disposables =>
        {
            HandleActivation(disposables);

            Disposable
                .Create(() => HandleDeactivation())
                .DisposeWith(disposables);
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        await ViewModel?.OnAppearingAsync();
        await ManageAppearingAnimationAsync(Width, Height);
    }

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        await ManageAppearingAnimationAsync(width, height);
    }

    private async Task ManageAppearingAnimationAsync(double width, double height)
    {
        if (!appearingAnimationDone && width > 0 && height > 0)
            await RunAppearingAnimationAsync();
    }

    public virtual Task RunAppearingAnimationAsync()
    {
        appearingAnimationDone = true;
        return Task.CompletedTask;
    }

    public virtual Task RunDisappearingAnimationAsync()
    {
        appearingAnimationDone = false;
        return Task.CompletedTask;
    }

    protected virtual void HandleActivation(CompositeDisposable disposables)
    {
        CreateBindings(disposables);
        ObserveValues(disposables);
        CreateEvents(disposables);
    }

    protected virtual void HandleDeactivation()
    { }

    protected virtual void CreateBindings(CompositeDisposable disposables)
    { }

    protected virtual void ObserveValues(CompositeDisposable disposables)
    { }

    protected virtual void CreateEvents(CompositeDisposable disposables)
    { }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ViewModel?.OnDisappearingAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        ViewModel?.NavigateBackCommand.Execute().Subscribe();
        return true;
    }
}
