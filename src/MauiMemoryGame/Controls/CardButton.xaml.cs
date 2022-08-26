using System.Windows.Input;

namespace MauiMemoryGame.Controls;

public partial class CardButton
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CardButton), propertyChanged: CommandChanged);
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(CardButton), propertyChanged: CornerRadiusChanged);
    public static readonly BindableProperty ButtonOnLeftProperty = BindableProperty.Create(nameof(ButtonOnLeft), typeof(bool), typeof(CardButton), propertyChanged: ButtonOnLeftChanged);
    public static readonly BindableProperty BackgroundSourceProperty = BindableProperty.Create(nameof(BackgroundSource), typeof(ImageSource), typeof(CardButton), propertyChanged: BackgroundSourceChanged);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CardButton), propertyChanged: TextChanged);
    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CardButton), defaultBindingMode: BindingMode.OneWay, propertyChanged: IsBusyChanged);

    public CardButton()
	{
		InitializeComponent();
	}

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public bool ButtonOnLeft
    {
        get => (bool)GetValue(ButtonOnLeftProperty);
        set => SetValue(ButtonOnLeftProperty, value);
    }

    public ImageSource BackgroundSource
    {
        get => (ImageSource)GetValue(BackgroundSourceProperty);
        set => SetValue(BackgroundSourceProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardButton)bindable).buttonContent.Command = (ICommand)newValue;
    }

    private static void CornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardButton)bindable).borderRectangle.CornerRadius = (CornerRadius)newValue;
    }

    private static void ButtonOnLeftChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CardButton cardButton = (CardButton)bindable;
        bool buttonOnLeft = (bool)newValue;

        cardButton.gridContent.ColumnDefinitions[0].Width = buttonOnLeft ? new GridLength(5, GridUnitType.Star) : new GridLength(1, GridUnitType.Star);
        cardButton.gridContent.ColumnDefinitions[1].Width = buttonOnLeft ? new GridLength(1, GridUnitType.Star) : new GridLength(5, GridUnitType.Star);
        Grid.SetColumn(cardButton.boxViewButtonBackground, buttonOnLeft ? 0 : 1);
        Grid.SetColumn(cardButton.buttonContent, buttonOnLeft ? 0 : 1);
    }

    private static void BackgroundSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardButton)bindable).imgBackground.Source = (ImageSource)newValue;
    }

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CardButton)bindable).buttonContent.Text = (string)newValue;
    }

    private static void IsBusyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CardButton control = (CardButton)bindable;
        bool value = (bool)newValue;

        control.buttonContent.Text = value ? string.Empty : control.Text;
        control.aiBusy.IsRunning = value;
        control.aiBusy.IsVisible = value;
    }
}
