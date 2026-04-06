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
    public class PublisherListVM
    {
        private readonly DbService db;
        public ObservableCollection<Publisher> Publishers { get; set; } = [];
        public ICommand NewCommand { get; set; }
        public ICommand SelectedPublisherCommand { get; set; }

        public PublisherListVM(DbService db)
        {
            this.db = db;

            NewCommand = new AsyncRelayCommand(NewPublisher);
            SelectedPublisherCommand = new AsyncRelayCommand<Publisher>(p => SelectPublisher(p!));
        }

        public async Task NewPublisher()
        {
            await Shell.Current.GoToAsync(nameof(PublisherForm));
        }

        public async Task SelectPublisher(Publisher? publisher)
        {
            if (publisher != null)
            {
                await Shell.Current.GoToAsync($"{nameof(PublisherForm)}?Id={publisher.Id}");
            }
        }

        public async Task LoadPublishers()
        {
            Publishers.Clear();
            var pubs = await db.GetAllPublishers();
            foreach(var p in pubs.OrderBy(p => p.Name))
            {
                Publishers.Add(p);
            }
        }
    }
}
