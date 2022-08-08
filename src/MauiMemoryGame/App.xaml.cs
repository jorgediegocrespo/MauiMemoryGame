using System.Globalization;

namespace MauiMemoryGame;

public partial class App : Application
{
	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
        MainPage = new AppShell(); //serviceProvider.GetService<ThemeSelectorView>();
	}

    protected override void OnStart()
    {
        base.OnStart();
        SetCulture(CultureInfo.CurrentCulture);
    }

    private void SetCulture(CultureInfo cultureInfo)
    {
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}
