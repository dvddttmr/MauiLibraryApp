using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class MediaList : ContentPage
{
	public MediaList(MediaListVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		var vm = (MediaListVM)BindingContext;
		await vm.LoadMedia();
    }
}