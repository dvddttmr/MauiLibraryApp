using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class AuthorVM : ObservableObject, IQueryAttributable
    {
        private Person author = new Person();

        private readonly DbService db;

        private string Source { get; set; }
        private int SourceId { get; set; }
        public ObservableCollection<string> RoleOptions { get; } =
            new ObservableCollection<string>()
            { "Author", "Illustrator", "Narrator", "Editor" };

        public string FirstName
        {
            get => author.FirstName;
            set
            {
                if(author.FirstName != value)
                {
                    author.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MiddleName
        {
            get => author.MiddleName;
            set
            {
                if(author.MiddleName != value)
                {
                    author.MiddleName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => author.LastName;
            set
            {
                if (author.LastName != value)
                {
                    author.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedRole
        {
            get => author.Role;
            set
            {
                if(author.Role != value)
                {
                    author.Role = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Biography
        {
            get => author.Biography;
            set
            {
                if(author.Biography != value)
                {
                    author.Biography = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Media> Works { get; set; } = new();

        
        public ICommand SeletedBookCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public AuthorVM(DbService db)
        {
            this.db = db;

            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
            CancelCommand = new AsyncRelayCommand(Cancel);
            SeletedBookCommand = new AsyncRelayCommand<Media>(SelectedBookChanged);
        }

        public AuthorVM(DbService db, Person author)
        {
            this.db = db;
            this.author = author;
            GetWorks();

            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
            CancelCommand = new AsyncRelayCommand(Cancel);
        }

        private async void GetWorks()
        {
            var mediaPersons = (await db.GetAllMediaPersons()).Where(mp => mp.PersonId == author.Id);
            foreach(var mp in mediaPersons)
            {
                Works.Add(await db.GetMediaById(mp.MediaId));
            }
        }

        public async Task SelectedBookChanged(Media media)
        {
            if(media != null)
            {
                await Shell.Current.GoToAsync($"{nameof(BookForm)}?Id={media.Id}");
            }
        }

        public async Task Save()
        {
            if(author.Id != 0)
            {
                await db.UpdatePerson(author);
            }
            else
            {
                await db.CreatePerson(author);
            }
            await Shell.Current.GoToAsync($"..?saved=true");
        }

        public async Task Delete()
        {
            await db.DeletePerson(author);
            await Shell.Current.GoToAsync($"..?deleted=true");
        }

        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var idObj))
            {
                int id = int.Parse(idObj.ToString());
                var auth = await db.GetPersonById(id);
                if (auth != null)
                {
                    author = auth;
                    RefreshProperties();
                }
                else
                {
                    throw new Exception("The author could not be found");
                }
            }

            if(query.TryGetValue("source", out var source))
            {
                this.Source = (string)source;
                if(query.TryGetValue("sourceId", out var sourceId))
                {
                    this.SourceId = (int)sourceId;
                }
            }
        }

        public async Task Reload()
        {
            author = await db.GetPersonById(author.Id);
            GetWorks();
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(MiddleName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(SelectedRole));
            OnPropertyChanged(nameof(Biography));
        }
    }
}
