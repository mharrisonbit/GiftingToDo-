using System.Collections.Generic;
using SQLite;

namespace GiftingToDo.Models
{
    [Table("Receiver")]
    public class Receiver
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
