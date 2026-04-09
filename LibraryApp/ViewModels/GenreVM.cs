using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryApp.Models;
using LibraryApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace LibraryApp.ViewModels
{
    public class GenreVM : ObservableObject
    {
        private readonly DbService db;

        public ObservableCollection<MediaType> MediaTypeList { get; set; } = [];
        public ObservableCollection<Genre> GenreList { get; set; } = [];

        private MediaType selectedMediaType;
        public MediaType SelectedMediaType
        {
            get => selectedMediaType;
            set
            {
                if(SetProperty(ref selectedMediaType, value))
                {
                    if(value != null)
                    {
                        _ = LoadGenres(value.Id);
                    }
                }
            }
        }

        private Genre selectedGenre;
        public Genre SelectedGenre
        {
            get => selectedGenre;
            set
            {
                if(SetProperty(ref selectedGenre, value))
                {
                    if(value != null)
                    {
                        NewGenreName = value.Name;
                    }
                }    
            }
        }

        private string newGenreName;
        public string NewGenreName
        {
            get => newGenreName;
            set => SetProperty(ref newGenreName, value);
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand BackCommand { get; set; }


        public GenreVM(DbService db)
        {
            this.db = db;
            DeleteCommand = new AsyncRelayCommand<Genre>(Delete);
            SaveCommand = new AsyncRelayCommand(Save);
            BackCommand = new AsyncRelayCommand(Back);

            _ = LoadMediaTypes();
        }

        public async Task Delete(Genre? genre)
        {
            if(genre != null)
            {
                await db.DeleteGenre(genre);
            }
            await LoadGenres(selectedMediaType.Id);
        }

        public async Task Save()
        {
            if (SelectedMediaType == null)
                return;

            if (string.IsNullOrWhiteSpace(NewGenreName))
                return;

            if(SelectedGenre == null)
            {
                //ADD
                var genre = new Genre() { MediaTypeId = selectedMediaType.Id, Name = NewGenreName };
                await db.CreateGenre(genre);
            }
            else
            {
                //UPDATE
                SelectedGenre.Name = NewGenreName;
                await db.UpdateGenre(SelectedGenre);
            }

            await LoadGenres(SelectedMediaType.Id);
        }

        public async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task LoadMediaTypes()
        {
            MediaTypeList.Clear();
            var mediaTypes = await db.GetAllMediaTypes();
            foreach (var mt in mediaTypes.OrderBy(m => m.Name))
            {
                MediaTypeList.Add(mt);
            }
        }

        public async Task LoadGenres(int mediaTypeId)
        {
            GenreList.Clear();
            var genres = await db.GetGenresForMediaType(mediaTypeId);
            foreach(var genre in genres.OrderBy(g => g.Name))
            {
                GenreList.Add(genre);
            }

            SelectedGenre = null;
            NewGenreName = string.Empty;
        }
    }
}
