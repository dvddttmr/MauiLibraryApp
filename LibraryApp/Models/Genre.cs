using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Models
{
    public class Genre
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public string Name { get; set; } = "";
    }
}
