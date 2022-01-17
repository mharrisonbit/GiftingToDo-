using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Models;

namespace GiftingToDo.Interfaces.Interfaces
{
    public interface IGiftingService
    {
        Task<bool> AddGiftToUserAsync(int id, Gift gifts);
        Task<bool> AddReceiverAsync(Receiver reciever);
        Task<string> CreateJsonForExport(Receiver reciever);
        Task<List<Receiver>> GetAllDeletedReciversAsync();
        Task<List<Gift>> GetAllGiftsForUser(int id);
        Task<List<Gift>> GetAllGiftsInDataBase();
        Task<List<Receiver>> GetAllReciversAsync();
        Task<List<Receiver>> GetCompeletedReciversAsync();
        Task<Receiver> GetRecieverAsync(int id);
        Task<int> GetRecieverCount();
        Task<int> GetRecieverCount(bool condition);
        Task<List<Receiver>> GetUncompeletedReciversAsync();
        Task<bool> ImportNewReciever(string infoString);
        Task RemoveAllGiftsFromDb();
        Task RemoveGiftFromReciever(Receiver receiver, Gift gift);
        Task RemoveRecieverAsync(int id);
        Task SetRecieverAsDeleted(int id);
        Task<bool> UpdateGiftInfo(Gift gift);
    }
}
