using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GiftingToDo.Models
{
    [Table("Gifts")]
    public class Gift
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public string PaperWrappedIn { get; set; }
        public double Price { get; set; }
        public bool ItemPurchased { get; set; }

        [ForeignKey(typeof(Receiver))]
        public int ReceiverId { get; set; }
    }
}
