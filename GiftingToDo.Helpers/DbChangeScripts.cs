using System;
using System.IO;
using System.Threading.Tasks;
using GiftingToDo.Models;
using SQLite;
using Xamarin.Essentials;

namespace GiftingToDo.Helpers
{
    public class DbChangeScripts
    {
        public SQLiteAsyncConnection _db { get; private set; }

        async Task Init()
        {
            try
            {
                if (_db != null)
                {
                    return;
                }
                var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "Gifting.db");
                _db = new SQLiteAsyncConnection(dataBasePath);

                await _db.CreateTablesAsync<Receiver, Gift>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// this is going to update the app based on the version that was passed to the method.
        /// </summary>
        /// <param name="versionNum"></param>
        public async Task DbUpdate(double versionNum)
        {
            try
            {
                if (versionNum == .1)
                {
                    await Init();
                    Preferences.Set("currentDbVersion", 0.1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Diagnostics.Debugger.Break();
            }
        }
    }
}
