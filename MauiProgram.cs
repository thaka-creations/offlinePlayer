using Microsoft.Extensions.Logging;

namespace tplayer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ViewModel.MainViewModel>();

            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<ViewModel.DetailViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
            
#endif
            return builder.Build();
        }
    }
}
