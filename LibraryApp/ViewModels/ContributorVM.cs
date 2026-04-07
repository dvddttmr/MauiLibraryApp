using CommunityToolkit.Mvvm.ComponentModel;
using LibraryApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.ViewModels
{
    public class ContributorVM: ObservableObject
    {
        public int Id { get; set; }
        public Person Person { get; set; } = new();
        private string role = "";
        public string Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }
        public Status Status { get; set; }
    }

    public enum Status
    {
        New,Existing,Delete
    }
}
