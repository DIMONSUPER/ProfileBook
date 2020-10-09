using SQLite;
using System;

namespace ProfileBook.Models
{
    public class ProfileModel
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public string Description { get; set; }
        public int UserId { get; set; }
        public string ProfileImage { get; set; }
        public string NickNameLabel { get; set; }
        public string NameLabel { get; set; }
        public DateTime DateLabel { get; set; }
    }
}
