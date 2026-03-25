using SQLite;

namespace LibraryApp.Models
{
    public class Media
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = "";
        //Foreign Key
        public int PublisherId { get; set; }
        public int PublicationYear { get; set; }
        public string Genre { get; set; } = "";
        public string MediaType { get; set; } = "";
    }
}
