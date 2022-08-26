namespace MauiMemoryGame.Features;

public partial class ThemeSelectorView
{
    public ThemeSelectorView(ThemeSelectorViewModel viewModel)
	{
        ViewModel = viewModel;
		InitializeComponent();
    }

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);
        btDc.HandleActivation(disposables);
        btMarvel.HandleActivation(disposables);
        btSimpson.HandleActivation(disposables);
        btStarWars.HandleActivation(disposables);
    }

    protected override void HandleDeactivation()
    {
        base.HandleDeactivation();
        btDc.HandleDeactivation();
        btMarvel.HandleDeactivation();
        btSimpson.HandleDeactivation();
        btStarWars.HandleDeactivation();
    }

    protected override void CreateBindings(CompositeDisposable disposables)
    {
        base.CreateBindings(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectDcCommand, v => v.btDc.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingDc, v => v.btDc.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectMarvelCommand, v => v.btMarvel.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingMarvel, v => v.btMarvel.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectSimpsonsCommand, v => v.btSimpson.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingSimpsons, v => v.btSimpson.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.SelectStarWarsCommand, v => v.btStarWars.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsSelectingStarWars, v => v.btStarWars.IsBusy).DisposeWith(disposables);
    }

    public override async Task RunAppearingAnimationAsync()
    {
        await base.RunAppearingAnimationAsync();

        using (var animation = new Animation())
        {
            double step = 0.5 / 4;

            animation.Add(0, 0.5, new Animation(x => lbTitle.Opacity = x, 0, 1));
            animation.Add(step * 1, 0.5 + step * 1, new Animation(x => btDc.Opacity = x, 0, 1));
            animation.Add(step * 2, 0.5 + step * 2, new Animation(x => btMarvel.Opacity = x, 0, 1));
            animation.Add(step * 3, 0.5 + step * 3, new Animation(x => btSimpson.Opacity = x, 0, 1));
            animation.Add(step * 4, 0.5 + step * 4, new Animation(x => btStarWars.Opacity = x, 0, 1));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "appearingAnimation", length: 1000, finished: (x, y) =>
            {
                lbTitle.Opacity = 1;
                btDc.Opacity = 1;
                btMarvel.Opacity = 1;
                btSimpson.Opacity = 1;
                btStarWars.Opacity = 1;

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

            animation.Add(0, 0.5, new Animation(x => lbTitle.Opacity = x, 1, 0));
            animation.Add(step * 1, 0.5 + step * 1, new Animation(x => btDc.Opacity = x, 1, 0));
            animation.Add(step * 2, 0.5 + step * 2, new Animation(x => btMarvel.Opacity = x, 1, 0));
            animation.Add(step * 3, 0.5 + step * 3, new Animation(x => btSimpson.Opacity = x, 1, 0));
            animation.Add(step * 4, 0.5 + step * 4, new Animation(x => btStarWars.Opacity = x, 1, 0));

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            animation.Commit(this, "disappearingAnimation", length: 1000, finished: (x, y) =>
            {
                lbTitle.Opacity = 0;
                btDc.Opacity = 0;
                btMarvel.Opacity = 0;
                btSimpson.Opacity = 0;
                btStarWars.Opacity = 0;

                tcs.SetResult(true);
            });

            await tcs.Task;
        }
    }
}

