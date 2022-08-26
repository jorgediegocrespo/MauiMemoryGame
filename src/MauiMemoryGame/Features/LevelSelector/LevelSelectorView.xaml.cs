namespace MauiMemoryGame.Features;

public partial class LevelSelectorView
{
	public LevelSelectorView(LevelSelectorViewModel viewModel)
	{
        ViewModel = viewModel;
        InitializeComponent();
	}

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);
        btBack.HandleActivation(disposables);
        btEasy.HandleActivation(disposables);
        btMedium.HandleActivation(disposables);
        btHigh.HandleActivation(disposables);
    }

    protected override void HandleDeactivation()
    {
        base.HandleDeactivation();
        btBack.HandleDeactivation();
        btEasy.HandleDeactivation();
        btMedium.HandleDeactivation();
        btHigh.HandleDeactivation();
    }

    protected override void CreateBindings(CompositeDisposable disposables)
    {
        base.CreateBindings(disposables);

        this.OneWayBind(ViewModel, vm => vm.NavigateBackCommand, v => v.btBack.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsNavigatingBack, v => v.btBack.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectHighCommand, v => v.btHigh.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingHigh, v => v.btHigh.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectMediumCommand, v => v.btMedium.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingMedium, v => v.btMedium.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectLowCommand, v => v.btEasy.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingLow, v => v.btEasy.IsBusy).DisposeWith(disposables);
    }

    public override async Task RunAppearingAnimationAsync()
    {
        await base.RunAppearingAnimationAsync();

        using (var animation = new Animation())
        {
            double step = 0.5 / 4;

            animation.Add(0, 0.5, new Animation(x => btBack.Opacity = x, 0, 1));
            animation.Add(step * 1, 0.5 + step * 1, new Animation(x => lbTitle.Opacity = x, 0, 1));
            animation.Add(step * 2, 0.5 + step * 2, new Animation(x => btEasy.Opacity = x, 0, 1));
            animation.Add(step * 3, 0.5 + step * 3, new Animation(x => btMedium.Opacity = x, 0, 1));
            animation.Add(step * 4, 0.5 + step * 4, new Animation(x => btHigh.Opacity = x, 0, 1));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "appearingAnimation", length: 1000, finished: (x, y) =>
            {
                btBack.Opacity = 1;
                lbTitle.Opacity = 1;
                btEasy.Opacity = 1;
                btMedium.Opacity = 1;
                btHigh.Opacity = 1;

                tcs.SetResult(true);
            });

            await tcs.Task;
        }
    }

    public override async Task RunDisappearingAnimationAsync()
    {
        await base.RunDisappearingAnimationAsync();

        using (var animation = new Animation())
        {
            double step = 0.5 / 4;

            animation.Add(0, 0.5, new Animation(x => btBack.Opacity = x, 1, 0));
            animation.Add(step * 1, 0.5 + step * 1, new Animation(x => lbTitle.Opacity = x, 1, 0));
            animation.Add(step * 2, 0.5 + step * 2, new Animation(x => btEasy.Opacity = x, 1, 0));
            animation.Add(step * 3, 0.5 + step * 3, new Animation(x => btMedium.Opacity = x, 1, 0));
            animation.Add(step * 4, 0.5 + step * 4, new Animation(x => btHigh.Opacity = x, 1, 0));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "disappearingAnimation", length: 1000, finished: (x, y) =>
            {
                btBack.Opacity = 0;
                lbTitle.Opacity = 0;
                btEasy.Opacity = 0;
                btMedium.Opacity = 0;
                btHigh.Opacity = 0;

                tcs.SetResult(true);
            });

            await tcs.Task;
        }
    }
}