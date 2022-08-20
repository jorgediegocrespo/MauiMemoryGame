using System.Reactive.Concurrency;

namespace MauiMemoryGame.Features;

public class GameViewModel : BaseViewModel, IQueryAttributable
{
    private IDisposable timer;

    public GameViewModel(ILogService logService, INavigationService navigationService) : base(logService, navigationService)
    {
    }

    [Reactive] public Themes SelectedTheme { get; set; }
    [Reactive] public Level SelectedLevel { get; set; }
    [Reactive] public Card[,] Board { get; set; }
    [Reactive] public bool GameOver { get; set; }
    [Reactive] public bool GameWon { get; set; }
    [Reactive] public int AttempsNumber { get; set; }
    [Reactive] public int CardPairsFount { get; set; }
    [Reactive] public TimeSpan RemainingTime { get; set; }

    public TimeSpan TotalTime => SelectedLevel switch
    {
        Level.Low => TimeSpan.FromSeconds(25), //TODO TimeSpan.FromMinutes(5),
        Level.Medium => TimeSpan.FromMinutes(4),
        Level.High => TimeSpan.FromMinutes(2),
        _ => throw new InvalidOperationException()
    };
    public int RowCount => SelectedLevel switch
    {
        Level.Low => 4,
        Level.Medium => 6,
        Level.High => 6,
        _ => throw new InvalidOperationException()
    };
    public int ColumnCount => SelectedLevel switch
    {
        Level.Low => 4,
        Level.Medium => 4,
        Level.High => 5,
        _ => throw new InvalidOperationException()
    };

    public ReactiveCommand<Unit, Unit> InitGameCommand { get; private set; }
    public extern bool IsInitiatingGame { [ObservableAsProperty] get; }

    public ReactiveCommand<Tuple<Card, Card>, bool> EqualsCardsCommand { get; private set; }
    public extern bool IsComparingCards{ [ObservableAsProperty] get; }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();

        disposables.Add(InitGameCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(InitGameCommand.IsExecuting.ToPropertyEx(this, x => x.IsInitiatingGame));

        disposables.Add(EqualsCardsCommand.ThrownExceptions.Subscribe(logService.TraceError));
        disposables.Add(EqualsCardsCommand.IsExecuting.ToPropertyEx(this, x => x.IsComparingCards));

        InitTimer();
    }

    public override async Task OnDisappearingAsync()
    {
        await base.OnDisappearingAsync();
        timer = null;
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        InitGameCommand = ReactiveCommand.Create(InitGame);
        EqualsCardsCommand = ReactiveCommand.CreateFromTask<Tuple<Card, Card>, bool>(EqualsCards);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedTheme = (Themes)query[nameof(SelectedTheme)];
        SelectedLevel = (Level)query[nameof(SelectedLevel)];

        InitGameCommand.Execute().Subscribe();
    }

    private void InitGame()
    {
        CreateBoard();
        InitValues();
    }

    private void InitValues()
    {
        GameOver = false;
        GameWon = false;
        AttempsNumber = 0;
        CardPairsFount = 0;
    }

    private void InitTimer()
    {
        if (timer != null)
            return;

        RemainingTime = RemainingTime.TotalSeconds > 0 ? RemainingTime : new TimeSpan(TotalTime.Ticks);

        timer = Observable.Interval(TimeSpan.FromSeconds(1))
            .TakeWhile(_ => RemainingTime > TimeSpan.Zero)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
                async _ =>
                {
                    RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                    if (RemainingTime.TotalSeconds == 0)
                        await FinishGame(false);
                });
        disposables.Add(timer);
    }

    private void CreateBoard()
    {
        Card[,] tmpBoard = new Card[RowCount, ColumnCount];

        int numberOfDistinctCards = RowCount * ColumnCount / 2;
        int numberOfFilledCards = 0;
        List<int> usedImages = new List<int>();

        string imagePath = SelectedTheme switch
        {
            Themes.DC => "dc_",
            Themes.Marvel => "marvel_",
            Themes.Simpsons => "simpsons_",
            Themes.StarWars => "star_wars_",
            _ => throw new InvalidOperationException()
        };

        while (numberOfFilledCards < numberOfDistinctCards)
        {
            int imageIndex = GetImageIndex(usedImages);
            usedImages.Add(imageIndex);

            Random random = new Random();
            FillBoardCell(tmpBoard, 0, $"{imagePath}{imageIndex}.jpg");
            FillBoardCell(tmpBoard, random.Next(RowCount * ColumnCount), $"{imagePath}{imageIndex}.jpg");

            numberOfFilledCards++;
        }

        Board = tmpBoard;
    }

    private int GetImageIndex(List<int> usedImages)
    {
        Random random = new Random();
        int imageIndex = random.Next(1, 15);

        if (!usedImages.Contains(imageIndex))
            return imageIndex;

        int i = imageIndex + 1;
        while (i <= 15)
        {
            if (!usedImages.Contains(i))
                return i;

            i++;
        }

        i = 1;
        while (i < imageIndex)
        {
            if (!usedImages.Contains(i))
                return i;

            i++;
        }

        throw new InvalidOperationException();
    }

    private void FillBoardCell(Card[,] tmpBoard, int index, string imagePath)
    {
        if (FillHigherBoardCell(tmpBoard, index, imagePath))
            return;

        if (FillLowerBoardCell(tmpBoard, index, imagePath))
            return;

        throw new InvalidOperationException();
    }

    private bool FillHigherBoardCell(Card[,] tmpBoard, int index, string imagePath)
    {
        int target = 0;
        for (int row = 0; row < RowCount; row++)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                if (target >= index && tmpBoard[row, column] == null)
                {
                    tmpBoard[row, column] = new Card { ImagePath = imagePath };
                    return true;
                }
                target++;
            }
        }

        return false;
    }

    private bool FillLowerBoardCell(Card[,] tmpBoard, int index, string imagePath)
    {
        int target = 0;
        for (int row = RowCount - 1; row >= 0; row--)
        {
            for (int column = ColumnCount - 1; column >= 0; column--)
            {
                if (target <= index && tmpBoard[row, column] == null)
                {
                    tmpBoard[row, column] = new Card { ImagePath = imagePath };
                    return true;
                }
                target++;
            }
        }

        return false;
    }

    private async Task<bool> EqualsCards(Tuple<Card, Card> cards)
    {
        AttempsNumber++;
        if (cards.Item1.ImagePath != cards.Item2.ImagePath)
            return false;

        cards.Item1.Fount = true;
        cards.Item2.Fount = true;
        CardPairsFount++;

        if (CardPairsFount == RowCount * ColumnCount / 2)
            await FinishGame(true);

        return true;
    }

    private async Task FinishGame(bool gameWon)
    {
        GameWon = gameWon;
        GameOver = true;

        timer?.Dispose();
        timer = null;

        bool playAgain = await navigationService.NavigateBackToGameOver(GameWon);
        if (playAgain)
            InitGameCommand.Execute().Subscribe();
        else
            await navigationService.NavigateBackToStart();
    }
}
