using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using LibraryApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class MediaTypeVm : ObservableObject
    {
        private readonly DbService db;
        private Models.MediaType selectedMediaType;

        public ObservableCollection<Models.MediaType> MediaTypeList { get; set; } = [];

        public MediaType SelectedMediaType
        {
            get => selectedMediaType;
            set
            {
                if(SetProperty(ref selectedMediaType, value))
                {
                    if(value != null)
                    {
                        NewMediaTypeName = value.Name;
                    }
                }
            }
        }
        private string newMediaTypeName;
        public string NewMediaTypeName
        {
            get => newMediaTypeName;
            set => SetProperty(ref newMediaTypeName, value);
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }


        public MediaTypeVm(DbService db)
        {
            this.db = db;
            DeleteCommand = new AsyncRelayCommand<MediaType>(Delete);
            SaveCommand = new AsyncRelayCommand(Save);
            CancelCommand = new AsyncRelayCommand(Cancel);

            Task.Run(LoadMediaTypeList);
        }

        public async Task Delete(MediaType? mt)
        {
            if (mt == null)
                return;

            await db.DeleteGenreByMediaType(mt.Id);
            await db.DeleteMediaType(mt);

            if (SelectedMediaType == mt)
            {
                SelectedMediaType = null;
                NewMediaTypeName = string.Empty;
            }

            await LoadMediaTypeList();
        }

        public async Task Save()
        {
            if (string.IsNullOrEmpty(NewMediaTypeName))
                return;

            if(SelectedMediaType == null)
            {
                //ADD
                var mt = new MediaType { Name = NewMediaTypeName };
                await db.CreateMediaType(mt);
            }
            else
            {
                //UPDATE
                SelectedMediaType.Name = NewMediaTypeName;
                await db.UpdateMediaType(SelectedMediaType);
            }

            SelectedMediaType = null;
            NewMediaTypeName = string.Empty;
            await LoadMediaTypeList();
        }

        public async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task LoadMediaTypeList()
        {
            MediaTypeList.Clear();
            var mediaTypes = await db.GetAllMediaTypes();
            foreach (var mt in mediaTypes.OrderBy(m => m.Name))
            {
                MediaTypeList.Add(mt);
            }
        }
    }
}
