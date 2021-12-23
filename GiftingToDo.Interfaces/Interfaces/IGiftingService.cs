using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Models;

namespace GiftingToDo.Interfaces.Interfaces
{
    public interface IGiftingService
    {
        Task AddGiftToUserAsync(Receiver receiver, Gift gift);
        Task AddReceiverAsync(Receiver receiver);
        Task<List<Receiver>> GetAllReciversAsync();
        Task GetRecieverAsync(int id);
        Task RemoveRecieverAsync(int id);
    }
}
