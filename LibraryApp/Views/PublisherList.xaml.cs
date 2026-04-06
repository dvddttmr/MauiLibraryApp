using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class PublisherList : ContentPage
{
	public PublisherList(PublisherListVM vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		var vm = (PublisherListVM)BindingContext;
		await vm.LoadPublishers();
    }
}