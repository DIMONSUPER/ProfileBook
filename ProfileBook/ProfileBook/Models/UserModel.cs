using SQLite;

namespace ProfileBook.Models
{
    [Table("Users")]
    public class UserModel
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; }

        [MaxLength(16)]
        public string Password { get; set; }
    }
}
