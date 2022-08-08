namespace MauiMemoryGame.Features;

public partial class CardView
{
    
	public static readonly BindableProperty CardProperty = BindableProperty.Create(nameof(Card), typeof(Card), typeof(CardView), defaultBindingMode: BindingMode.OneWay, propertyChanged: CardChanged);

    public CardView()
	{
		InitializeComponent();
	}

    public Card Card
    {
        get => (Card)GetValue(CardProperty);
        set => SetValue(CardProperty, value);
    }

    public async Task ShowContent()
    {
        frContent.RotationY = -270;
        await frBackwards.RotateYTo(-90);
        frBackwards.IsVisible = false;
        frContent.IsVisible = true;
        await frContent.RotateYTo(-360);
        frContent.RotationY = 0;
    }

    public async Task HideContent()
    {
        frBackwards.RotationY = -270;
        await frContent.RotateYTo(-90);
        frContent.IsVisible = false;
        frBackwards.IsVisible = true;
        await frBackwards.RotateYTo(-360);
        frBackwards.RotationY = 0;
    }

    private static void CardChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Card newCard = newValue as Card;
        ((CardView)bindable).imgContent.Source = newCard != null ? 
            ImageSource.FromFile(newCard.ImagePath) : 
            null;
    }
}