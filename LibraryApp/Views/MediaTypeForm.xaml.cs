using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class MediaTypeForm : ContentPage
{
	public MediaTypeForm(MediaTypeVm vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}