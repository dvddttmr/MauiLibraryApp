using CommunityToolkit.Mvvm.Input;
using LibraryApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class AdminVM
    {
        public ICommand GenreCommand { get; set; }
        public ICommand MediaTypeCommand { get; set; }
        //public ICommand BackCommand { get; set; }

        public AdminVM()
        {
            GenreCommand = new AsyncRelayCommand(Genre);
            MediaTypeCommand = new AsyncRelayCommand(MediaType);
            //BackCommand = new AsyncRelayCommand(Back);
        }

        public async Task Genre() => await Shell.Current.GoToAsync(nameof(GenreForm));

        public async Task MediaType() => await Shell.Current.GoToAsync(nameof(MediaTypeForm));

        //public async Task Back() => await Shell.Current.GoToAsync("..");


    }
}
