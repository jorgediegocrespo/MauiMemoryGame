namespace MauiMemoryGame.Features;

public partial class GameView
{
	private CardView firstPairCard;

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
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.AttempsNumber, v => v.lbAttemps.Text)); 
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.CardPairsFount, v => v.lbPairs.Text)); 
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.lbTimer.Text, x => $"{x.Minutes}:{x.Seconds}"));
        disposables.Add(this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.timeProgress.ProgressPercentage, x => GetTimePercentage(x)));
    }

    protected override void ObserveValues(CompositeDisposable disposables)
	{
		base.ObserveValues(disposables);

		disposables.Add(this.WhenAnyValue(x => x.ViewModel.Board)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(BuildBoard));

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

	private void BuildBoard(Card[,] board)
	{
		try
		{
			IsDrawingBoard = true;
			if (board == null)
				return;

			CreateGridBoard();
			FillGridBoard(board);
		}
		finally
		{
			IsDrawingBoard = false;
        }
	}

	private void FillGridBoard(Card[,] board)
	{
		for (int row = 0; row < ViewModel.RowCount; row++)
		{
			for (int column = 0; column < ViewModel.ColumnCount; column++)
			{
				CardView cardView = new CardView { Card = board[row, column] };
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
				cardView.GestureRecognizers.Add(tapGestureRecognizer);
				gridBoard.Add(cardView, column, row);
			}
		}
	}

	private void CreateGridBoard()
	{
		CreateGridBoardRows();
		CreateGridBoardColumns();

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

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		CardView selectedCard = (CardView)sender;
		if (selectedCard.IsShowingContent)
			return;

        if (firstPairCard == selectedCard)
            return;

        _ = selectedCard.ShowContent();
        if (firstPairCard == null)
		{
			firstPairCard = selectedCard;
			return;
		}		

		ViewModel.EqualsCardsCommand
			.Execute(new Tuple<Card, Card>(firstPairCard.Card, selectedCard.Card))
			.Subscribe(async equals =>
			{
				if (equals)
				{
                    firstPairCard = null;
                    return;
                }	

				await Task.Delay(2000);
				await Task.WhenAll(
					selectedCard.HideContent(),
					firstPairCard.HideContent());

				firstPairCard = null;
			});
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