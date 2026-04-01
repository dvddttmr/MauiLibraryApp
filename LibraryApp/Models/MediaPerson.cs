using SQLite;

namespace LibraryApp.Models
{
    public class MediaPerson
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public int MediaId { get; set; }
        public int PersonId { get; set; }
        public string Role { get; set; } = "";
    }
}
