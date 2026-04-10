using LibraryApp.Views;

namespace LibraryApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AuthorForm), typeof(AuthorForm));
            Routing.RegisterRoute(nameof(PublisherForm), typeof(PublisherForm));
            Routing.RegisterRoute(nameof(MediaForm), typeof(MediaForm));
            Routing.RegisterRoute(nameof(GenreForm), typeof(GenreForm));
            Routing.RegisterRoute(nameof(MediaTypeForm), typeof(MediaTypeForm));
        }
    }
}
