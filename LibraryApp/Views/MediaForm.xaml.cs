using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class MediaForm : ContentPage
{
	public MediaForm(MediaVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}