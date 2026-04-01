using LibraryApp.ViewModels;

namespace LibraryApp.Views;

public partial class AuthorList : ContentPage
{
	public AuthorList(AuthorListVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		var vm = (AuthorListVM)BindingContext;
		vm.LoadAuthors();
    }
}