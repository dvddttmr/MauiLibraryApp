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
    public class MediaListVM: ObservableObject
    {
        private readonly DbService db;
        public ObservableCollection<Media> MediaList { get; set; } = [];
        public ICommand NewMediaCommand { get; set; }
        public ICommand SelectedMediaCommand { get; set; }

        public MediaListVM(DbService db)
        {
            this.db = db;
            NewMediaCommand = new AsyncRelayCommand(NewMedia);
            SelectedMediaCommand = new AsyncRelayCommand<Media>(SelectedBook);
        }

        public async Task LoadMedia()
        {
            MediaList.Clear();
            var media = await db.GetAllMedia();
            foreach(var m in media)
            {
                MediaList.Add(m);
            }
        }

        public async Task NewMedia()
        {
            await Shell.Current.GoToAsync(nameof(MediaForm));
        }

        public async Task SelectedBook(Media? media)
        {
            if(media != null)
                await Shell.Current.GoToAsync($"{nameof(MediaForm)}?Id={media.Id}");
        }
    }
}
