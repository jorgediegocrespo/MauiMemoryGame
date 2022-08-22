namespace MauiMemoryGame.Features;

public partial class GameView
{
	private CardView firstPairCard;
	private bool isManagingCards;


    public GameView(GameViewModel viewModel)
	{
        ViewModel = viewModel;
		IsDrawingBoard = false;
        InitializeComponent();
	}

    [Reactive] public bool IsDrawingBoard { get; set; }

	protected override void CreateBindings(CompositeDisposable disposables)
	{
		base.CreateBindings(disposables);

        disposables.Add(this.OneWayBind(ViewModel, vm => vm.NavigateBackCommand, v => v.btBack.Command));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.AttempsNumber, v => v.lbAttemps.Text)); 
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.CardPairsFount, v => v.lbPairs.Text)); 
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.lbTimer.Text, x => $"{x.Minutes.ToString().PadLeft(2, '0')}:{x.Seconds.ToString().PadLeft(2, '0')}"));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.timeProgress.ProgressPercentage, x => GetTimePercentage(x)));
    }

    protected override void ObserveValues(CompositeDisposable disposables)
	{
		base.ObserveValues(disposables);

		disposables.Add(this.WhenAnyValue(x => x.ViewModel.Board)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(x => BuildBoard(x, disposables)));

        disposables.Add(this.WhenAnyValue(x => x.ViewModel.IsInitiatingGame, x => x.IsDrawingBoard)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe((x) => ManageBoardVisibility(x.Item1 || x.Item2)));
    }

	private float GetTimePercentage(TimeSpan remainingTime)
	{
		float totalSeconds = (float)ViewModel.TotalTime.TotalSeconds;
		if (totalSeconds == 0)
			return 100;

		float remainingTimeSeconds = (float)remainingTime.TotalSeconds;
		float result = (remainingTimeSeconds * 100f) / totalSeconds;
        return result;
    }

	private void BuildBoard(Card[,] board, CompositeDisposable disposables)
	{
		try
		{
			IsDrawingBoard = true;
			if (board == null)
				return;

			CreateGridBoard();
			FillGridBoard(board, disposables);
		}
		finally
		{
			IsDrawingBoard = false;
        }
	}

	private void FillGridBoard(Card[,] board, CompositeDisposable disposables)
	{
		for (int row = 0; row < ViewModel.RowCount; row++)
		{
			for (int column = 0; column < ViewModel.ColumnCount; column++)
			{
				CardView cardView = new CardView { Card = board[row, column] };
                IObservable<EventPattern<object>> cardViewClicked = Observable.FromEventPattern(h => cardView.Clicked += h, h => cardView.Clicked -= h);
                disposables.Add(cardViewClicked.Subscribe(x => TapGestureRecognizer_Tapped(x.Sender, null)));

                gridBoard.Add(cardView, column, row);
            }
		}
	}

    private void CreateGridBoard()
	{
		ClearGrid();
		CreateGridBoardRows();
		CreateGridBoardColumns();
    }

	private void ClearGrid()
	{
		gridBoard.RowDefinitions.Clear();
		gridBoard.ColumnDefinitions.Clear();
	}

	private void CreateGridBoardRows()
    {
		for (int row = 0; row < ViewModel.RowCount; row++)
			gridBoard.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
    }

    private void CreateGridBoardColumns()
    {
        for (int column = 0; column < ViewModel.ColumnCount; column++)
            gridBoard.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
    }

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		if (isManagingCards)
			return;

		CardView selectedCard = (CardView)sender;
		if (selectedCard.IsShowingContent)
			return;

        if (firstPairCard == selectedCard)
            return;

		isManagingCards = true;
        await selectedCard.ShowContent();
        if (firstPairCard == null)
		{
			firstPairCard = selectedCard;
            isManagingCards = false;
            return;
		}		

		ViewModel.EqualsCardsCommand
			.Execute(new Tuple<Card, Card>(firstPairCard.Card, selectedCard.Card))
			.Subscribe(async areEquals =>
			{
				await ManageCardEqualsResult(areEquals, selectedCard);
                isManagingCards = false;
            });
	}

    private async Task ManageCardEqualsResult(bool areEquals, CardView secondPairCard)
	{
        if (areEquals)
        {
            firstPairCard = null;
            return;
        }

        await Task.Delay(2000);
        await Task.WhenAll(
            secondPairCard.HideContent(),
            firstPairCard.HideContent());

        firstPairCard = null;
    }

	private void ManageBoardVisibility(bool isBuildingBoard)
	{
		aiCreatingBoard.IsRunning = isBuildingBoard;
        aiCreatingBoard.IsVisible = isBuildingBoard;
		gridBoard.IsVisible = !isBuildingBoard;
		frPairs.IsVisible = !isBuildingBoard;
        frAttemps.IsVisible = !isBuildingBoard;
        frTimer.IsVisible = !isBuildingBoard;
    }
}