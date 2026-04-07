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
        private Person person = new Person();

        private readonly DbService db;

        public string[] RoleOptions { get; } = { "Author", "Illustrator", "Narrator", "Editor", "Contributor"};

        public string FirstName
        {
            get => person.FirstName;
            set
            {
                if(person.FirstName != value)
                {
                    person.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MiddleName
        {
            get => person.MiddleName;
            set
            {
                if(person.MiddleName != value)
                {
                    person.MiddleName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => person.LastName;
            set
            {
                if (person.LastName != value)
                {
                    person.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Title
        {
            get => person.Title;
            set
            {
                if(person.Title != value)
                {
                    person.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Suffix
        {
            get => person.Suffix;
            set
            {
                if(person.Suffix != value)
                {
                    person.Suffix = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PreferredName
        {
            get => person.PreferredName;
            set
            {
                if(person.PreferredName != value)
                {
                    person.PreferredName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime BirthDate
        {
            get => person.BirthDate;
            set
            {
                if(person.BirthDate != value)
                {
                    person.BirthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isDeceased;
        public bool IsDeceased 
        { 
            get => isDeceased;
            set
            {
                if (isDeceased != value)
                {
                    isDeceased = value;
                    OnPropertyChanged();
                }
            } 
        }

        public DateTime? DeathDate =>
            IsDeceased ? SelectedDeathDate : null;

        private DateTime selectedDeathDate = DateTime.Today;
        public DateTime SelectedDeathDate 
        { 
            get => selectedDeathDate; 
            set
            {
                if(selectedDeathDate != value)
                {
                    selectedDeathDate = value;
                    OnPropertyChanged();
                }
            } 
        }

        public string? ImagePath
        {
            get => person.ImagePath;
            set
            {
                person.ImagePath = value;
                OnPropertyChanged();
            }
        }

        public string Biography
        {
            get => person.Biography;
            set
            {
                if(person.Biography != value)
                {
                    person.Biography = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Media> Works { get; set; } = new();

        
        public ICommand BrowseImagesCommand { get; set; }
        public ICommand SeletedBookCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public AuthorVM(DbService db)
        {
            this.db = db;

            BrowseImagesCommand = new AsyncRelayCommand(BrowseImages);
            SaveCommand = new AsyncRelayCommand(Save);
            DeleteCommand = new AsyncRelayCommand(Delete);
            CancelCommand = new AsyncRelayCommand(Cancel);
            SeletedBookCommand = new AsyncRelayCommand<Media>(SelectedBookChanged);
        }

        private async void GetWorks()
        {
            var mediaPersons = (await db.GetAllMediaPersons()).Where(mp => mp.PersonId == person.Id);
            foreach(var mp in mediaPersons)
            {
                Works.Add(await db.GetMediaById(mp.MediaId));
            }
        }

        public async Task BrowseImages()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select an Image",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            ImagePath = result.FullPath;
        }

        public async Task SelectedBookChanged(Media media)
        {
            if(media != null)
            {
                await Shell.Current.GoToAsync($"{nameof(MediaForm)}?Id={media.Id}");
            }
        }

        public async Task Save()
        {
            //process DeathDate
            person.DeathDate = IsDeceased ? SelectedDeathDate : null;

            bool isNew = person.Id == 0;

            //1.  Process New Persons
            if(isNew)
            {
                person.Id = await db.CreatePerson(person);
            }

            //2. Process Image
            if (ImagePath != null)
            {
                var fileName = $"{person.Id}.jpg";
                var savePath = Path.Combine(FileSystem.AppDataDirectory, "PersonFaces", fileName);
                ImageService.ResizeAndCrop(
                    sourcePath: ImagePath,
                    destinationPath: savePath,
                    targetWidth: 150,
                    targetHeight: 200);
                ImagePath = savePath;
            }

            //3. Update the Person (For both new and existing)
            await db.UpdatePerson(person);

            //
            await Shell.Current.GoToAsync($"..?saved=true");
        }

        public async Task Delete()
        {
            await db.DeletePerson(person);
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
                await LoadPerson(id);
            }
        }

        public async Task LoadPerson(int id)
        {
            this.person = await db.GetPersonById(id);
            if(person.DeathDate.HasValue)
            {
                IsDeceased = true;
                SelectedDeathDate = (DateTime)person.DeathDate;
            }
            else
            {
                IsDeceased = false;
            }
            GetWorks();
            RefreshProperties();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(FirstName));
            OnPropertyChanged(nameof(MiddleName));
            OnPropertyChanged(nameof(LastName));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Suffix));
            OnPropertyChanged(nameof(PreferredName));
            OnPropertyChanged(nameof(BirthDate));
            OnPropertyChanged(nameof(DeathDate));
            OnPropertyChanged(nameof(ImagePath));
            OnPropertyChanged(nameof(Biography));
        }
    }
}
