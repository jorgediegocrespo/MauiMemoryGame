﻿namespace MauiMemoryGame.Features;

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

        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectDcCommand, v => v.btDc));
        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectMarvelCommand, v => v.btMarvel));
        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectSimpsonsCommand, v => v.btSimpson));
        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectStarWarsCommand, v => v.btStarWars));
    }
}
