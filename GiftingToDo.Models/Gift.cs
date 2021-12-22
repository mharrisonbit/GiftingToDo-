using System;
using SQLite;

namespace GiftingToDo.Models
{
    [Table("Gifts")]
    public class Gift
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public string ItemType { get; set; }
        public double Price { get; set; }

        [ForeignKey(typeof(Receiver))]
        public int ReceiverId { get; set; }
    }
}
