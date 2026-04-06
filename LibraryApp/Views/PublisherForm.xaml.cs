using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class PublisherForm : ContentPage
{
	public PublisherForm(PublisherVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}