namespace MauiMemoryGame.Features;

public partial class ThemeSelectorView
{
    public ThemeSelectorView(ThemeSelectorViewModel viewModel)
	{
        ViewModel = viewModel;
		InitializeComponent();
    }

    protected override void CreateBindings(CompositeDisposable disposables)
    {
        base.CreateBindings(disposables);

        disposables.Add(this.OneWayBind(ViewModel, vm => vm.SelectDcCommand, v => v.btDc.Command));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.SelectMarvelCommand, v => v.btMarvel.Command));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.SelectSimpsonsCommand, v => v.btSimpson.Command));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.SelectStarWarsCommand, v => v.btStarWars.Command));
    }
}

