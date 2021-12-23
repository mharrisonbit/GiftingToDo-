using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using SQLite;
using Xamarin.Essentials;

namespace GiftingToDo.Interfaces.Implementations
{
    public class GiftingService : IGiftingService
    {
        private readonly IErrorHandler errorHandler;

        public GiftingService(IErrorHandler errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        public SQLiteAsyncConnection _db { get; private set; }

        async Task Init()
        {
            try
            {
                if (_db is not null)
                {
                    return;
                }
                var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "Gifting.db");
                _db = new SQLiteAsyncConnection(dataBasePath);

                await _db.CreateTablesAsync<Receiver, Gift>();
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        /// <summary>
        /// this is going to add the new person and gifts to the DB
        /// </summary>
        /// <param name="reciever"></param>
        /// <returns></returns>
        public async Task AddReceiverAsync(Receiver reciever)
        {
            try
            {
                await Init();

                await _db.InsertAsync(reciever);
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        /// <summary>
        /// this is going to delete the user that is associated to the ID that is passed to it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveRecieverAsync(int id)
        {
            try
            {
                await Init();

                await _db.DeleteAsync<Receiver>(id);
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        /// <summary>
        /// this is going to return one person based on the ID that is passed to it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task GetRecieverAsync(int id)
        {
            await Init();
        }

        /// <summary>
        /// This gets and returns all the people that are saved in the DB.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Receiver>> GetAllReciversAsync()
        {
            try
            {
                await Init();

                var recievers = await _db.Table<Receiver>().ToListAsync();
                TotalAmountSpent(recievers);
                return recievers;
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }

            return null;
        }

        /// <summary>
        /// this is going to add the gift to the user that was passed.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="gift"></param>
        /// <returns></returns>
        public async Task AddGiftToUserAsync(Receiver receiver, Gift gift)
        {
            await Init();
            //TODO add the logic to add the gift to the user that was passed.
        }

        public async Task RemoveGiftFromReciever(Receiver receiver, Gift gift)
        {
            await Init();
            //TODO remove the gift from the user in question.
        }

        /// <summary>
        /// This is going to loop through the list of recievers and then do the math to see if the gift has been purchased and figure the amount spent.
        /// </summary>
        /// <param name="receivers"></param>
        private void TotalAmountSpent(List<Receiver> receivers)
        {
            try
            {
                foreach (var receiver in receivers)
                {
                    var totalSpent = 0.0;
                    foreach (var gift in receiver.Gifts)
                    {
                        if (gift.ItemPurchased)
                        {
                            totalSpent += gift.Price;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }
    }
}
