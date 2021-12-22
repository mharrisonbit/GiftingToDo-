using System;
using System.IO;
using System.Threading.Tasks;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using SQLite;
using Xamarin.Essentials;

namespace GiftingToDo.Interfaces.Implementations
{
    public class GiftingService : IGiftingService
    {
        public SQLiteAsyncConnection db { get; private set; }

        public async Task Init()
        {
            if (db is not null)
            {
                return;
            }
            var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "Gifting.db");
            db = new SQLiteAsyncConnection(dataBasePath);

            await db.CreateTableAsync<Receiver>();
        }


    }
}
