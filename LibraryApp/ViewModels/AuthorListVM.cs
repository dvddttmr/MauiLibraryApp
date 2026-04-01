using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class AuthorListVM
    {
        private readonly DbService db;
        public ObservableCollection<Person> AuthorList { get; set; } = [];
        public ICommand NewCommand { get; }
        public ICommand SelectAuthorCommand { get; }
        
        public AuthorListVM(DbService db)
        {
            this.db = db;
            //LoadAuthors();
            NewCommand = new AsyncRelayCommand(NewAuthor);
            SelectAuthorCommand = new AsyncRelayCommand<Person>(SelectAuthor);
        }

        public async void LoadAuthors()
        {
            AuthorList.Clear();
            var authors = await db.GetAllPersons();
            foreach(var auth in authors)
            {
                AuthorList.Add(auth);
            }
            AuthorList.OrderBy(a => a.LastName);
        }

        public async Task NewAuthor()
        {
            await Shell.Current.GoToAsync(nameof(AuthorForm));
        }

        public async Task SelectAuthor(Person person)
        {
            if(person != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AuthorForm)}?id={person.Id}");
            }
        }
    }
}
