using LibraryApp.ViewModels;
using LibraryApp.Models;

namespace LibraryApp.Views;

public partial class GenreForm : ContentPage
{
	public GenreForm(GenreVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}