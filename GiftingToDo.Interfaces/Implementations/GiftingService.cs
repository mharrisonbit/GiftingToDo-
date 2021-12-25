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
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        #region Reciever based methods
        /// <summary>
        /// this is going to add the new person and gifts to the DB
        /// </summary>
        /// <param name="reciever"></param>
        /// <returns></returns>
        public async Task<bool> AddReceiverAsync(Receiver reciever)
        {
            var personAdded = true;
            try
            {
                await Init();

                await _db.InsertAsync(reciever);
                if (reciever.Gifts != null)
                {
                    var added = await AddGiftToUserAsync(reciever.Id, reciever.Gifts);
                }
            }
            catch (Exception ex)
            {
                personAdded = false;
                this.errorHandler.PrintErrorMessage(ex);
            }
            return personAdded;
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

                var usersGifts = await GetAllGiftsForRecieverAsync(id);

                foreach (var item in usersGifts)
                {
                    await _db.DeleteAsync<Gift>(item.Id);
                }

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
        public async Task<Receiver> GetRecieverAsync(int id)
        {
            await Init();

            var reciever = await _db.GetAsync<Receiver>(id);

            List<Gift> giftsList = await GetAllGiftsForRecieverAsync(reciever.Id);
            if (giftsList != null)
            {
                reciever.Gifts = giftsList;
            }

            return reciever;
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
                foreach (var reciever in recievers)
                {
                    List<Gift> giftsList = await GetAllGiftsForRecieverAsync(reciever.Id);
                    if (giftsList != null)
                    {
                        reciever.Gifts = giftsList;
                    }
                    
                }
                TotalAmountSpent(recievers);
                await IsRecieverFinished(recievers);
                return recievers;
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }

            return null;
        }

        /// <summary>
        /// This gets and returns all the people that have all the gifts saved in the DB.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Receiver>> GetCompeletedReciversAsync()
        {
            try
            {
                await Init();

                var recievers = await _db.Table<Receiver>().Where(x => x.IsComplete == true).ToListAsync();
                foreach (var reciever in recievers)
                {
                    List<Gift> giftsList = await GetAllGiftsForRecieverAsync(reciever.Id);
                    if (giftsList != null)
                    {
                        reciever.Gifts = giftsList;
                    }

                }
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
        /// This gets and returns all the people that have all the gifts saved in the DB.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Receiver>> GetUncompeletedReciversAsync()
        {
            try
            {
                await Init();

                var recievers = await _db.Table<Receiver>().Where(x => x.IsComplete == false).ToListAsync();

                foreach (var reciever in recievers)
                {
                    List<Gift> giftsList = await GetAllGiftsForRecieverAsync(reciever.Id);
                    if (giftsList != null)
                    {
                        reciever.Gifts = giftsList;
                    }

                }

                TotalAmountSpent(recievers);
                await IsRecieverFinished(recievers);
                return recievers;
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }

            return null;
        }
        #endregion Reciever based methods.

        #region Gift based methods
        /// <summary>
        /// this is going to add the gift to the user that was passed.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="gift"></param>
        /// <returns></returns>
        public async Task<bool> AddGiftToUserAsync(int id, Gift gift)
        {
            var giftAdded = true;
            try
            {
                await Init();
                gift.ReceiverId = id;
                await _db.InsertAsync(gift);
            }
            catch (Exception ex)
            {
                giftAdded = false;
                this.errorHandler.PrintErrorMessage(ex);
            }
            return giftAdded;
        }

        /// <summary>
        /// this is going to add the List of gifts to the user that was passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gifts"></param>
        /// <returns></returns>
        public async Task<bool> AddGiftToUserAsync(int id, List<Gift> gifts)
        {
            var giftAdded = true;
            try
            {
                await Init();
                foreach (var gift in gifts)
                {
                    gift.ReceiverId = id;
                }
                await _db.InsertAllAsync(gifts);

            }
            catch (Exception ex)
            {
                giftAdded = false;
                this.errorHandler.PrintErrorMessage(ex);
            }
            return giftAdded;
        }

        public async Task<bool> UpdateGiftInfo(Gift gift)
        {
            var updated = true;
            try
            {
                await Init();
                var giftToUpdate = await _db.GetAsync<Gift>(gift.Id);
                await _db.UpdateAsync(gift);
                var giftUpdated = await _db.GetAsync<Gift>(gift.Id);
                await IsRecieverFinished(gift.ReceiverId);
            }
            catch (Exception ex)
            {
                updated = false;
                this.errorHandler.PrintErrorMessage(ex);
            }

            return updated;
        }

        /// <summary>
        /// this is going to add a gift and link it to the reciever that is sent to it.
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="gift"></param>
        /// <returns></returns>
        public async Task RemoveGiftFromReciever(Receiver receiver, Gift gift)
        {
            try
            {
                await Init();

                gift.ReceiverId = receiver.Id;
                await _db.DeleteAsync<Gift>(gift);
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        public async Task RemoveAllGiftsFromDb()
        {
            try
            {
                await Init();
                await _db.ExecuteAsync("Delete From Gifts");
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        public async Task<List<Gift>> GetAllGiftsInDataBase()
        {
            var gifts = await _db.Table<Gift>().ToListAsync();
            return gifts;
        }
        #endregion Gift based methods

        #region these are the hepler methods
        private async Task<List<Gift>> GetAllGiftsForRecieverAsync(int id)
        {
            await Init();
            var gifts = await _db.Table<Gift>().Where(c => c.ReceiverId == id).ToListAsync();
            return gifts;
        }

        /// <summary>
        /// This is going to loop through the list of recievers and then do the math to see if the gift has been purchased and figure the amount spent.
        /// </summary>
        /// <param name="recievers"></param>
        private void TotalAmountSpent(List<Receiver> recievers)
        {
            try
            {
                foreach (var receiver in recievers)
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

        private async Task IsRecieverFinished(List<Receiver> recievers)
        {
            foreach (var reciever in recievers)
            {
                var giftCount = reciever.Gifts.Count;
                if (giftCount >= 1)
                {
                    var giftsPurchased = 0;
                    foreach (var gift in reciever.Gifts)
                    {
                        if (gift.ItemPurchased)
                        {
                            giftsPurchased++;
                        }
                    }

                    if (giftsPurchased == giftCount)
                    {
                        reciever.IsComplete = true;
                    }

                    if (reciever.IsComplete)
                    {
                        await _db.UpdateAsync(reciever);
                    }
                }
            }
        }

        private async Task IsRecieverFinished(Receiver reciever)
        {
            var giftCount = reciever.Gifts.Count;
            if (giftCount >= 1)
            {
                var giftsPurchased = 0;
                foreach (var gift in reciever.Gifts)
                {
                    if (gift.ItemPurchased)
                    {
                        giftsPurchased++;
                    }
                }

                if (giftsPurchased == giftCount)
                {
                    reciever.IsComplete = true;
                }

                if (reciever.IsComplete)
                {
                    await _db.UpdateAsync(reciever);
                }
            }
        }

        private async Task IsRecieverFinished(int recieverId)
        {
            try
            {
                var reciever = await GetRecieverAsync(recieverId);
                var giftCount = reciever.Gifts.Count;
                if (giftCount >= 1)
                {
                    var giftsPurchased = 0;
                    foreach (var gift in reciever.Gifts)
                    {
                        if (gift.ItemPurchased)
                        {
                            giftsPurchased++;
                        }
                    }

                    reciever.IsComplete = giftsPurchased == giftCount;
                }

                if (reciever.IsComplete)
                {
                    await _db.UpdateAsync(reciever);
                }
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }
        #endregion end of the helper methods.
    }
}
