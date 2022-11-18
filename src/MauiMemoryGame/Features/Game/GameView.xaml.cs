namespace MauiMemoryGame.Features;

public partial class GameView
{
	private List<CardView> cards;
	private CardView firstPairCard;
	private bool isManagingCards;


    public GameView(GameViewModel viewModel)
	{
        ViewModel = viewModel;
		cards = new List<CardView>();
        InitializeComponent();
	}

    protected override void HandleActivation(CompositeDisposable disposables)
    {
        base.HandleActivation(disposables);
        btBack.HandleActivation(disposables);
    }

    protected override void HandleDeactivation()
    {
        base.HandleDeactivation();
        btBack.HandleDeactivation();
        cards.ForEach(x => x.HandleDeactivation());
    }

    protected override void CreateBindings(CompositeDisposable disposables)
	{
		base.CreateBindings(disposables);

        this.OneWayBind(ViewModel, vm => vm.NavigateBackCommand, v => v.btBack.Command).DisposeWith(disposables);
        this.OneWayBind(ViewModel, vm => vm.IsNavigatingBack, v => v.btBack.IsBusy).DisposeWith(disposables);

        this.OneWayBind(ViewModel, vm => vm.AttempsNumber, v => v.lbAttemps.Text).DisposeWith(disposables);
		this.OneWayBind(ViewModel, vm => vm.CardPairsFount, v => v.lbPairs.Text).DisposeWith(disposables);
		this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.lbTimer.Text, x => $"{x.Minutes.ToString().PadLeft(2, '0')}:{x.Seconds.ToString().PadLeft(2, '0')}").DisposeWith(disposables);
		this.OneWayBind(ViewModel, vm => vm.RemainingTime, v => v.timeProgress.ProgressPercentage, x => GetTimePercentage(x)).DisposeWith(disposables);
    }

    protected override void ObserveValues(CompositeDisposable disposables)
	{
		base.ObserveValues(disposables);

		this.WhenAnyValue(x => x.ViewModel.Board)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(x => BuildBoard(x, disposables))
			.DisposeWith(disposables);

        this.WhenAnyValue(x => x.ViewModel.IsInitiatingGame, x => x.ViewModel.IsBoardLoaded)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(async x => await ManageBoardVisibilityAsync(x.Item1 || !x.Item2))
            .DisposeWith(disposables);
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
		if (board == null)
			return;

		CreateGridBoard();
		FillGridBoard(board, disposables);
        ViewModel.IsBoardLoaded = true;
    }

	private void FillGridBoard(Card[,] board, CompositeDisposable disposables)
	{
		for (int row = 0; row < ViewModel.RowCount; row++)
		{
			for (int column = 0; column < ViewModel.ColumnCount; column++)
			{
				CardView cardView = new CardView { Card = board[row, column] };
				cardView.HandleActivation(disposables);
				cards.Add(cardView);

#if ANDROID
                Observable
                    .FromEventPattern(h => cardView.Clicked += h, h => cardView.Clicked -= h)
                    .Subscribe(x => TapGestureRecognizer_Tapped(x.Sender, null))
                    .DisposeWith(disposables);
#else
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                cardView.GestureRecognizers.Add(tapGestureRecognizer);

				Observable
					.FromEventPattern<TappedEventArgs>(h => tapGestureRecognizer.Tapped += h, h => tapGestureRecognizer.Tapped -= h)
					.Subscribe(x => TapGestureRecognizer_Tapped(x.Sender, null))
					.DisposeWith(disposables);

#endif

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

        await Task.Delay(1500);
        await Task.WhenAll(
            secondPairCard.HideContent(),
            firstPairCard.HideContent());

        firstPairCard = null;
    }

	private async Task ManageBoardVisibilityAsync(bool isBuildingBoard)
	{
        bool showControls = aiCreatingBoard.IsVisible && !isBuildingBoard;

        aiCreatingBoard.IsRunning = isBuildingBoard;
        aiCreatingBoard.IsVisible = isBuildingBoard;
		gridBoard.IsVisible = !isBuildingBoard;
		frPairs.IsVisible = !isBuildingBoard;
        frAttemps.IsVisible = !isBuildingBoard;
        frTimer.IsVisible = !isBuildingBoard;

		if (showControls)
		{
			await CustomRunAppearingAnimationAsync();
			ViewModel.InitTimerCommand.Execute().Subscribe();
		}
	}

	private async Task CustomRunAppearingAnimationAsync()
	{
		await base.RunAppearingAnimationAsync();

		using (var animation = new Animation())
		{
			double step = 0.5 / 4;

			animation.Add(0, 0.5, new Animation(x => btBack.Opacity = x, 0, 1));
			animation.Add(step * 1, 0.5 + step * 1, new Animation(x => frPairs.Opacity = x, 0, 1));
			animation.Add(step * 2, 0.5 + step * 2, new Animation(x => frAttemps.Opacity = x, 0, 1));
			animation.Add(step * 3, 0.5 + step * 3, new Animation(x => frTimer.Opacity = x, 0, 1));
			animation.Add(step * 4, 0.5 + step * 4, new Animation(x => gridBoard.Opacity = x, 0, 1));

			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			animation.Commit(this, "appearingAnimation", length: 1000, finished: (x, y) =>
			{
				btBack.Opacity = 1;
				frPairs.Opacity = 1;
				frAttemps.Opacity = 1;
				frTimer.Opacity = 1;
				gridBoard.Opacity = 1;

				tcs.SetResult(true);
			});

			await tcs.Task;
		}
	}

	public override async Task RunDisappearingAnimationAsync()
	{
		await base.RunDisappearingAnimationAsync();

		using (var animation = new Animation())
		{
			double step = 0.5 / 4;

			animation.Add(0, 0.5, new Animation(x => btBack.Opacity = x, 1, 0));
			animation.Add(step * 1, 0.5 + step * 1, new Animation(x => frPairs.Opacity = x, 1, 0));
			animation.Add(step * 2, 0.5 + step * 2, new Animation(x => frAttemps.Opacity = x, 1, 0));
			animation.Add(step * 3, 0.5 + step * 3, new Animation(x => frTimer.Opacity = x, 1, 0));
			animation.Add(step * 4, 0.5 + step * 4, new Animation(x => gridBoard.Opacity = x, 1, 0));

			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			animation.Commit(this, "disappearingAnimation", length: 1000, finished: (x, y) =>
			{
				btBack.Opacity = 0;
				frPairs.Opacity = 0;
				frAttemps.Opacity = 0;
				frTimer.Opacity = 0;
				gridBoard.Opacity = 0;

				tcs.SetResult(true);
			});

			await tcs.Task;
		}
	}
}