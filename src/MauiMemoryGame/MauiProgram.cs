using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MauiMemoryGame;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        MauiAppBuilder builder = MauiApp.CreateBuilder();
		builder
			.UseSkiaSharp()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.Services
			.RegisterServices()
            .RegisterViewsAndViewModels();

        return builder.Build();
	}

	private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ILogService, LogService>();
    }

    private static IServiceCollection RegisterViewsAndViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<ThemeSelectorViewModel>().AddTransient<ThemeSelectorView>()
            .AddTransient<LevelSelectorViewModel>().AddTransient<LevelSelectorView>()
            .AddTransient<GameViewModel>().AddTransient<GameView>();
    }
}
