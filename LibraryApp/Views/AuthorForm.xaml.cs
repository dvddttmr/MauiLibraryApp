using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class AuthorForm : ContentPage
{
	public AuthorForm(AuthorVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}