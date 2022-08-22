using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;

namespace MauiMemoryGame;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();

        builder
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIcons");
            })
            .Services.RegisterServices()
            .RegisterViewsAndViewModels();

        return builder.Build();
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ILogService, LogService>()
            .AddSingleton<IDialogService, DialogService>();
    }

    private static IServiceCollection RegisterViewsAndViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<ThemeSelectorViewModel>().AddTransient<ThemeSelectorView>()
            .AddTransient<LevelSelectorViewModel>().AddTransient<LevelSelectorView>()
            .AddTransient<GameViewModel>().AddTransient<GameView>();
    }
}