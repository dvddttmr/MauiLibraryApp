using LibraryApp.Services;
using LibraryApp.ViewModels;
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
            builder.Services.AddSingleton(provider =>
            {
                return new DbService(path);
            });
            builder.Services.AddTransient<AuthorListVM>();
            builder.Services.AddTransient<AuthorVM>();
            builder.Services.AddTransient<PublisherListVM>();
            builder.Services.AddTransient<PublisherVM>();
            builder.Services.AddTransient<MediaListVM>();
            builder.Services.AddTransient<MediaVM>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            var db = app.Services.GetService<DbService>();
            if(db != null)
                Task.Run(async () => await db.Init());

            return app;
        }
    }
}
