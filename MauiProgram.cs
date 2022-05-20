using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Dispatching;

namespace RxMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMarkup()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var app = builder.Build();

		var dispatcher = app.Services.GetRequiredService<IDispatcher>();
		RxApp.MainThreadScheduler = new MauiScheduler(dispatcher);

		return app;
	}
}
