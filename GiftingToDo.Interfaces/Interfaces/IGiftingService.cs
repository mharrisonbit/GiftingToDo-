using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Models;

namespace GiftingToDo.Interfaces.Interfaces
{
    public interface IGiftingService
    {
        Task AddGiftToUserAsync(Receiver receiver, List<Gift> gifts);
        Task AddReceiverAsync(Receiver reciever);
        Task<List<Gift>> GetAllGiftsInDataBase();
        Task<List<Receiver>> GetAllReciversAsync();
        Task<Receiver> GetRecieverAsync(int id);
        Task RemoveAllGiftsFromDb();
        Task RemoveGiftFromReciever(Receiver receiver, Gift gift);
        Task RemoveRecieverAsync(int id);
    }
}
