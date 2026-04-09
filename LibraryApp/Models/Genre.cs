using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryApp.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public string Name { get; set; } = "";
    }
}
