using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class BookForm : ContentPage
{
	public BookForm(BookVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}