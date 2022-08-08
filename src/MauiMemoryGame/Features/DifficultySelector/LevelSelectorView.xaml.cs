namespace MauiMemoryGame.Features;

public partial class LevelSelectorView
{
	public LevelSelectorView(LevelSelectorViewModel viewModel)
	{
        ViewModel = viewModel;
        InitializeComponent();
	}

    protected override void CreateBindings(CompositeDisposable disposables)
    {
        base.CreateBindings(disposables);

        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectHighCommand, v => v.btHigh));
        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectMediumCommand, v => v.btMedium));
        disposables.Add(this.BindCommand(ViewModel, vm => vm.SelectLowCommand, v => v.btEasy));
    }
}