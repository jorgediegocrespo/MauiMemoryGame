using System.Windows.Input;

namespace MauiMemoryGame.Controls;

public partial class CustomButton
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomButton), defaultBindingMode: BindingMode.OneWay, propertyChanged: TextChanged);
    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CustomButton), defaultBindingMode: BindingMode.OneWay, propertyChanged: IsBusyChanged);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomButton), defaultBindingMode: BindingMode.OneWay, propertyChanged: CommandChanged);

    public event EventHandler ButtonClicked;

    public CustomButton()
    {
        InitializeComponent();
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

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void CreateBindings(CompositeDisposable disposables)
    {
        base.CreateBindings(disposables);

        Observable
            .FromEventPattern(h => btCustom.Clicked += h, h => btCustom.Clicked -= h)
            .Subscribe(x => ButtonClicked?.Invoke(this, null))
            .DisposeWith(disposables);
    }

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CustomButton)bindable).btCustom.Text = (string)newValue;
    }


    private static void IsBusyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        CustomButton control = (CustomButton)bindable;
        bool value = (bool)newValue;

        control.btCustom.Text = value ? string.Empty : control.Text;
        control.aiBusy.IsRunning = value;
        control.aiBusy.IsVisible = value;
    }

    private static void CommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CustomButton)bindable).btCustom.Command = (ICommand)newValue;
    }
}