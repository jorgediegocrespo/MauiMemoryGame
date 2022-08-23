namespace MauiMemoryGame.Base;

public class BasePopup<TViewModel> : BaseContentPage<TViewModel> where TViewModel : BaseViewModel
{
    public static readonly BindableProperty PopupContentProperty = BindableProperty.Create(nameof(PopupContent), typeof(View), typeof(BasePopup<TViewModel>), defaultValue: null, defaultBindingMode: BindingMode.OneWay, propertyChanged: PopupContentPropertyChanged);

    private ContentView contentView;

    public BasePopup()
    {
        Grid gridContent = new Grid();
        gridContent.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
        gridContent.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

        contentView = new ContentView();
        gridContent.Add(contentView, 0, 0);

        this.Content = gridContent;
        this.BackgroundColor = Color.FromArgb("#80000000");
    }

    public View PopupContent
    {
        get => (View)GetValue(PopupContentProperty);
        set { SetValue(PopupContentProperty, value); }
    }

    private static void PopupContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var controls = (BasePopup<TViewModel>)bindable;
        if (newValue != null)
            controls.contentView.Content = (View)newValue;
    }
}
