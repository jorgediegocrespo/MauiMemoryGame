namespace MauiMemoryGame.Features;

public class GameViewModel : BaseViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private IDisposable timer;

    public GameViewModel(ILogService logService, INavigationService navigationService, IDialogService dialogService) : base(logService, navigationService)
    {
        this.dialogService = dialogService;
    }

    public Themes SelectedTheme { get; private set; }
    public Level SelectedLevel { get; private set; }
    [Reactive] public Card[,] Board { get; private set; }
    [Reactive] public bool GameOver { get; private set; }
    [Reactive] public bool GameWon { get; private set; }
    [Reactive] public int AttempsNumber { get; private set; }
    [Reactive] public int CardPairsFount { get; private set; }
    [Reactive] public TimeSpan RemainingTime { get; private set; }
    [Reactive] public bool IsBoardLoaded { get; set; }

    public TimeSpan TotalTime => SelectedLevel switch
    {
        Level.Low => TimeSpan.FromMinutes(5),
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

    public ReactiveCommand<Unit, Unit> InitTimerCommand { get; private set; }
    public extern bool IsInitiatingTime { [ObservableAsProperty] get; }

    public ReactiveCommand<Tuple<Card, Card>, bool> EqualsCardsCommand { get; private set; }
    public extern bool IsComparingCards{ [ObservableAsProperty] get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        SelectedTheme = (Themes)query[nameof(SelectedTheme)];
        SelectedLevel = (Level)query[nameof(SelectedLevel)];
        RemainingTime = new TimeSpan(RemainingTime.Ticks);

        InitGameCommand.Execute().Subscribe();
    }

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);

        InitGameCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        InitGameCommand.IsExecuting.ToPropertyEx(this, x => x.IsInitiatingGame).DisposeWith(disposables);

        InitTimerCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        InitTimerCommand.IsExecuting.ToPropertyEx(this, x => x.IsInitiatingTime).DisposeWith(disposables);

        EqualsCardsCommand.ThrownExceptions.Subscribe(logService.TraceError).DisposeWith(disposables);
        EqualsCardsCommand.IsExecuting.ToPropertyEx(this, x => x.IsComparingCards).DisposeWith(disposables);
    }

    protected override void HandleDeactivation()
    {
        base.HandleDeactivation();
        timer?.Dispose();
        timer = null;
    }

    public override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        InitTimerCommand.Execute().Subscribe();
    }

    protected override void CreateCommands()
    {
        base.CreateCommands();

        InitGameCommand = ReactiveCommand.Create(InitGame);
        InitTimerCommand = ReactiveCommand.Create(InitTimer);
        EqualsCardsCommand = ReactiveCommand.CreateFromTask<Tuple<Card, Card>, bool>(EqualsCards);
    }

    protected override async Task NavigateBackAsync()
    {
        timer?.Dispose();
        timer = null;

        bool goBack = await dialogService.ShowDialogConfirmationAsync(TextsResource.GameBackQuestionTitle, TextsResource.GameBackQuestionMessage, TextsResource.Cancel, TextsResource.Ok);
        if (goBack)
            await base.NavigateBackAsync();
        else
            InitTimer();
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
        if (timer != null || !IsBoardLoaded)
            return;

        RemainingTime = RemainingTime.TotalSeconds > 0 ? RemainingTime : new TimeSpan(TotalTime.Ticks);

        timer = Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(
                async _ =>
                {
                    RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
                    if (RemainingTime.TotalSeconds == 0)
                        await FinishGame(false);
                });
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

        await navigationService.NavigateToGameOverPopup(SelectedTheme, SelectedLevel, GameWon);
    }
}
