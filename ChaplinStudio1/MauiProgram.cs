using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;

namespace ChaplinStudio1
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
                });
            //builder.Services.AddFilePicker();
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}
