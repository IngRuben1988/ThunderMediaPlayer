using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using Microsoft.Extensions.Logging;
using ThunderMediaPlayer.Src;
namespace ThunderMediaPlayer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkitMediaElement()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif	

		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddSingleton<MainPage>();


		return builder.Build();
	}
}
