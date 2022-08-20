using CommunityToolkit.Maui.Views;
using SkiaSharp.Extended.UI.Controls;

namespace MauiMemoryGame.Features;

public partial class GameOverView : Popup
{
    private bool isWinner;
    private CompositeDisposable disposables;

    public GameOverView(bool isWinner)
	{
		InitializeComponent();
        
        this.isWinner = isWinner;
        CompleteComponent();

        disposables = new CompositeDisposable();
        CreateEvents();
    }

    ~GameOverView()
    {
        disposables?.Dispose();
    }

    private void CompleteComponent()
    {
        if (isWinner)
        {
            skiaLottie.Source = new SKFileLottieImageSource { File = "win.json" };
            lbTitle.Text = TextsResource.GameWonTitle;
            lbSubtitle.Text = TextsResource.GameWonSubtitle;
        }
        else
        {
            skiaLottie.Source = new SKFileLottieImageSource { File = "lose.json" };
            lbTitle.Text = TextsResource.GameLoseTitle;
            lbSubtitle.Text = TextsResource.GameLoseSubtitle;
        }
        
    }

    private void CreateEvents()
    {
        IObservable<EventPattern<object>> btCustomClicked = Observable.FromEventPattern(h => btClose.Clicked += h, h => btClose.Clicked -= h);
        disposables.Add(btCustomClicked.Subscribe(x => Close(false)));

        IObservable<EventPattern<object>> btPlayAgainClicked = Observable.FromEventPattern(h => btPlayAgain.Clicked += h, h => btPlayAgain.Clicked -= h);
        disposables.Add(btPlayAgainClicked.Subscribe(x => Close(true)));
    }
}