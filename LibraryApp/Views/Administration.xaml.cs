using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class Administration : ContentPage
{
	public Administration(AdminVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}