using LibraryApp.Services;
using Microsoft.Extensions.Logging;

namespace LibraryApp
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
            var path = Path.Combine(FileSystem.AppDataDirectory, "Library.db3");
            builder.Services.AddSingleton(new DbService(path));

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            if(File.Exists(path))
            {
                var db = app.Services.GetService<DbService>();
                db.Init().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
