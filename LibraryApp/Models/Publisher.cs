using SQLite;

namespace LibraryApp.Models
{
    public class Publisher
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string LogoPath { get; set; } = "";
        public string About { get; set; } = "";
    }
}
