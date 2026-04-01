using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Models
{
    public class Person
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Title { get; set; } = "";
        public string Suffix { get; set; } = "";
        public string PreferredName { get; set; } = "";
        [Ignore]
        public string FullName => $"{LastName}, {FirstName} {MiddleName}";
        public DateTime BirthDate { get; set; } = new();
        public DateTime? DeathDate { get; set; } = null;
        public string? ImagePath { get; set; } = null;
        public string Biography { get; set; } = "";
    }
}
