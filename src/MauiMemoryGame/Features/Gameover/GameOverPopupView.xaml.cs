using SkiaSharp.Extended.UI.Controls;

namespace MauiMemoryGame.Features;

public partial class GameOverPopupView
{
	public GameOverPopupView(GameOverPopupViewModel viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);
        btClose.HandleActivation(disposables);
        btPlayAgain.HandleActivation(disposables);
    }

    protected override void HandleDeactivation()
    {
        base.HandleDeactivation();
        btClose.HandleDeactivation();
        btPlayAgain.HandleDeactivation();
    }

    protected override void CreateBindings(CompositeDisposable disposables)
	{
		base.CreateBindings(disposables);

        this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.skiaLottie.Source, x => x ? new SKFileLottieImageSource { File = "win.json" } : new SKFileLottieImageSource { File = "lose.json" }).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.lbTitle.Text, x => x ? TextsResource.GameWonTitle : TextsResource.GameLoseTitle).DisposeWith(disposables);
		this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.lbSubtitle.Text, x => x ? TextsResource.GameWonSubtitle : TextsResource.GameLoseSubtitle).DisposeWith(disposables);
        
        this.OneWayBind(ViewModel, vm => vm.NavigateBackCommand, v => v.btClose.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsNavigatingBack, v => v.btClose.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.PlayAgainCommand, v => v.btPlayAgain.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsGoingToPlayAgain, v => v.btPlayAgain.IsBusy).DisposeWith(disposables);
    }
}