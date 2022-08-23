using SkiaSharp.Extended.UI.Controls;

namespace MauiMemoryGame.Features;

public partial class GameOverPopupView
{
	public GameOverPopupView(GameOverPopupViewModel viewModel)
	{
		ViewModel = viewModel;
		InitializeComponent();
	}

	protected override void CreateBindings(CompositeDisposable disposables)
	{
		base.CreateBindings(disposables);

        disposables.Add(this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.skiaLottie.Source, x => x ? new SKFileLottieImageSource { File = "win.json" } : new SKFileLottieImageSource { File = "lose.json" }));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.lbTitle.Text, x => x ? TextsResource.GameWonTitle : TextsResource.GameLoseTitle));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.IsWinner, v => v.lbSubtitle.Text, x => x ? TextsResource.GameWonSubtitle : TextsResource.GameLoseSubtitle));

        disposables.Add(this.OneWayBind(ViewModel, vm => vm.NavigateBackCommand, v => v.btClose.Command));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.PlayAgainCommand, v => v.btPlayAgain.Command));
    }
}