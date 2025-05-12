using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notes.ViewModels;
using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Views;

namespace Notes;

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

			var a = Assembly.GetExecutingAssembly();
			using var stream = a.GetManifestResourceStream("Notes.appsettings.json");
				
			var config = new ConfigurationBuilder()
				.AddJsonStream(stream)
				.Build();
    
			builder.Configuration.AddConfiguration(config);

			var connectionString = builder.Configuration.GetConnectionString("LocalConnection");
			builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlServer(connectionString));

			builder.Services.AddSingleton<AllNotesViewModel>();
			builder.Services.AddTransient<NoteViewModel>();

			builder.Services.AddSingleton<AllNotesPage>();
			builder.Services.AddTransient<NotePage>();

			#if DEBUG
					builder.Logging.AddDebug();
			#endif

					return builder.Build();
	}
	
}
