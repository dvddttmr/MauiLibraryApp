using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class PublisherVM : ObservableObject, IQueryAttributable
    {
        private readonly DbService db;
        private Publisher publisher = new();

        #region Properties
        public string Name
        {
            get => publisher.Name;
            set
            {
                if(publisher.Name != value)
                {
                    publisher.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string City 
        { 
            get => publisher.City; 
            set
            {
                if(publisher.City != value)
                {
                    publisher.City = value;
                    OnPropertyChanged();
                }
            }
        }

        public string State
        {
            get => publisher.State;
            set
            {
                if(publisher.State != value)
                {
                    publisher.State = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Country
        {
            get => publisher.Country;
            set
            {
                if (publisher.Country != value)
                {
                    publisher.Country = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? LogoPath
        {
            get => publisher.LogoPath;
            set
            {
                if(publisher.LogoPath != value)
                {
                    publisher.LogoPath = value;
                    OnPropertyChanged();
                }
            }
        }

        public string About
        {
            get => publisher.About;
            set
            {
                if(publisher.About != value)
                {
                    publisher.About = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Media> Works { get; set; } = new();

        #endregion

        #region Command Declarations

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand BrowseLogoCommand { get; set; }
        public ICommand SelectWorkCommand { get; set; }

        #endregion

        public PublisherVM(DbService db)
        {
            this.db = db;

            DeleteCommand = new AsyncRelayCommand(Delete);
            SaveCommand = new AsyncRelayCommand(Save);
            CancelCommand = new AsyncRelayCommand(Cancel);
            BrowseLogoCommand = new AsyncRelayCommand(BrowseForLogo);
            SelectWorkCommand = new AsyncRelayCommand<Media>(SelectedWorkChanged);
        }

        #region Command Definitions

        public async Task Delete()
        {
            await db.DeletePublisher(publisher);
            await Shell.Current.GoToAsync("..?deleted=true");
        }

        public async Task Save()
        {
            if (publisher.Id == 0)
                publisher.Id = await db.CreatePublisher(publisher);

            if (LogoPath != null)
            {
                var filePath = $"{publisher.Id}.jpg";
                var savePath = Path.Combine(FileSystem.AppDataDirectory, "PublisherLogos",filePath);

                ImageService.ResizeAndCrop(
                    sourcePath: LogoPath,
                    destinationPath:savePath,
                    targetWidth:200,
                    targetHeight:200
                    );

                LogoPath = savePath;
            }

            await db.UpdatePublisher(publisher);

            await Shell.Current.GoToAsync("..?saved=true");
        }

        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task BrowseForLogo()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select an Image",
                FileTypes = FilePickerFileType.Images
            });

            if (result == null)
                return;

            LogoPath = result.FullPath;
        }

        public async Task SelectedWorkChanged(Media media)
        {
            await Shell.Current.GoToAsync($"{nameof(BookForm)}?Id={media.Id}");
        }

        #endregion

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if(query.TryGetValue("Id", out var idObj))
            {
                await LoadPublisher((int)idObj);
            }
        }

        private async Task LoadPublisher(int id)
        {
            publisher = await db.GetPublisherById(id);
            await GetWorks();
            RefreshProperties();
        }

        public async Task GetWorks()
        {
            Works = (await db.GetAllMedia()).Where(m => m.PublisherId == publisher.Id).ToList();
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(City));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(Country));
            OnPropertyChanged(nameof(LogoPath));
            OnPropertyChanged(nameof(About));
        }
    }
}
