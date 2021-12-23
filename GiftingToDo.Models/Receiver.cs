﻿using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

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
        public double AmountSpent { get; set; }
        public double TotalAmountSpent { get; set; }

        [OneToMany]
        public List<Gift> Gifts { get; set; }
    }
}