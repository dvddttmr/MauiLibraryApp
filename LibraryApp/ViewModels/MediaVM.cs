using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class MediaVM: ObservableObject, IQueryAttributable
    {
        private Media media;
        private readonly DbService db;
        public ObservableCollection<MediaType> MediaTypes { get; set; } = [];
        public ObservableCollection<Person> Persons { get; set; } = [];
        public ObservableCollection<Publisher> Publishers { get; set; } = [];

        public string SelectedMediaType
        {
            get => media.MediaType;
            set
            {
                if (media.MediaType != value)
                {
                    media.MediaType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => media.Title;
            set
            {
                if(media.Title != value)
                {
                    media.Title = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ContributorVM> Contributors { get; set; } = [];

        public int PublicationYear
        {
            get => media.PublicationYear;
            set
            {
                if(media.PublicationYear != value)
                {
                    media.PublicationYear = value;
                    OnPropertyChanged();
                }
            }
        }        

        public string Genre
        {
            get => media.Genre;
            set
            {
                if(media.Genre != value)
                {
                    media.Genre = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public MediaVM(DbService db)
        {
            this.db = db;

            CancelCommand = new AsyncRelayCommand(Cancel);
            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
        }

        public async Task Delete()
        {
            await db.DeleteMediaPersonForMedia(media.Id);
            await db.DeleteMedia(media);
            await Shell.Current.GoToAsync("..?Deleted=true");
        }

        public async Task Save()
        {
            var isNew = media.Id == 0;
            if (isNew)
                media.Id = await db.CreateMedia(media);

            await SaveContributors();

            await db.UpdateMedia(media);

            await Shell.Current.GoToAsync("..?Saved=true");
        }

        public async Task SaveContributors()
        {
            foreach (var c in Contributors)
            {
                switch (c.Status)
                {
                    case Status.New:
                        await db.CreateMediaPerson(new MediaPerson
                        {
                            MediaId = media.Id,
                            PersonId = c.Person.Id,
                            Role = c.Role
                        });
                        break;

                    case Status.Existing:
                        await db.UpdateMediaPerson(new MediaPerson
                        {
                            Id = c.Id,
                            MediaId = media.Id,
                            PersonId = c.Person.Id,
                            Role = c.Role
                        });
                        break;

                    case Status.Delete:
                        await db.DeleteMediaPerson(c.Id);
                        break;
                }
            }

        }

        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async void LoadLists()
        {
            await GetMediaTypes();
            await GetPersons();
            await GetPublishers();
        }

        public async Task GetMediaTypes()
        {
            MediaTypes.Clear();
            var mediaTypes = await db.GetAllMediaTypes();
            foreach(var mt in mediaTypes.OrderBy(m => m.Name))
            {
                MediaTypes.Add(mt);
            }
        }
        public async Task GetPersons()
        {
            Persons.Clear();
            var auths = await db.GetAllPersons();
            foreach(var a in auths.OrderBy(p => p.FullName))
            {
                Persons.Add(a);
            }
        }

        public async Task GetPublishers()
        {
            Publishers.Clear();
            var pubs = await db.GetAllPublishers();
            foreach(var p in pubs.OrderBy(p => p.Name))
            {
                Publishers.Add(p);
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Id", out var idObj))
            {
                if (int.TryParse(idObj.ToString(), out int id))
                    await LoadMedia(id);
            }
        }

        public async Task LoadMedia(int id)
        {
            this.media = await db.GetMediaById(id);
            await LoadContributors();
            await RefreshProperties();
            LoadLists();
        }

        public async Task LoadContributors()
        {
            Contributors.Clear();
            var mps = await db.GetMediaPersonsForMedia(media.Id);
            foreach(var mp in mps)
            {
                Contributors.Add(new ContributorVM()
                {
                    Id = mp.Id,
                    Person = await db.GetPersonById(mp.PersonId),
                    Role = mp.Role,
                    Status = Status.Existing
                });
            }
        }

        public async Task RefreshProperties()
        {
            OnPropertyChanged(nameof(MediaType));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Contributors));
            OnPropertyChanged(nameof(PublicationYear));
            OnPropertyChanged(nameof(Genre));
        }
    }
}
