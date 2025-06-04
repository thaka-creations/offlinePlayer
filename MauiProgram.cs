using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace tplayer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
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

            // authentication
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ViewModel.LoginViewModel>();

            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ViewModel.HomeViewModel>();

            builder.Services.AddTransient<PlayerPage>();
            builder.Services.AddTransient<ViewModel.PlayerViewModel>();

            builder.Services.AddTransient<AccountPage>();
            builder.Services.AddTransient<ViewModel.AccountViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
            
#endif
            return builder.Build();
        }
    }
}
