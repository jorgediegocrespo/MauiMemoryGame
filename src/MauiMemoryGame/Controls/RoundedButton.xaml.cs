using System.Windows.Input;

namespace MauiMemoryGame.Controls;

public partial class RoundedButton : ContentView
{
    private CompositeDisposable disposables;

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RoundedButton), propertyChanged: CommandChanged);
    public static readonly BindableProperty ButtonTypeProperty = BindableProperty.Create(nameof(ButtonType), typeof(RoundedButtonType), typeof(RoundedButton), defaultBindingMode: BindingMode.OneWay, defaultValue: RoundedButtonType.Back, propertyChanged: ButtonTypeChanged);

    public event EventHandler Clicked;

    public RoundedButton()
	{
		InitializeComponent();
     
        disposables = new CompositeDisposable();
        CreateBindings();
        RefreshIcon();
    }

    ~RoundedButton()
    {
        disposables?.Dispose();
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public RoundedButtonType ButtonType
    {
        get => (RoundedButtonType)GetValue(ButtonTypeProperty);
        set => SetValue(ButtonTypeProperty, value);
    }

    private void CreateBindings()
    {
        IObservable<EventPattern<object>> btCustomClicked = Observable.FromEventPattern(h => button.Clicked += h, h => button.Clicked -= h);
        disposables.Add(btCustomClicked.Subscribe(x => Clicked?.Invoke(this, null)));
    }

    private void RefreshIcon()
    {
        buttonSource.Glyph = ButtonType switch
        {
            RoundedButtonType.Back => "\ue5c4",
            RoundedButtonType.Close => "\ue5cd",
            _ => throw new InvalidOperationException()
        };
    }

    private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((RoundedButton)bindable).button.Command = (ICommand)newValue;
    }

    private static void ButtonTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((RoundedButton)bindable).RefreshIcon();
    }
}

public enum RoundedButtonType
{
    Back,
    Close
}