using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class AddGiftViewModel : ViewModelBase, INavigationAware
    {
        public DelegateCommand AddGiftToRecieverBtn { get; private set; }

        public int recieverId { get; private set; }
        public IGiftingService giftingService { get; }

        public AddGiftViewModel(INavigationService navigationService, IErrorHandler errorHandler, IGiftingService giftingService) : base(navigationService, errorHandler)
        {
            this.giftingService = giftingService;
            AddGiftToRecieverBtn = new DelegateCommand(async ()=> await AddGiftToUser());
            GiftToAdd = new Gift();
        }

        Gift _GiftToAdd;
        public Gift GiftToAdd
        {
            get { return _GiftToAdd; }
            set { SetProperty(ref _GiftToAdd, value); }
        }

        private async Task AddGiftToUser()
        {
            
            var wasAdded = await this.giftingService.AddGiftToUserAsync(recieverId, GiftToAdd);
            if (wasAdded)
            {
                GiftToAdd.ItemType = null;
                GiftToAdd.ItemPurchased = false;
                GiftToAdd.ItemDescription = null;
                GiftToAdd.Price = 0.0;
                GiftToAdd.PaperWrappedIn = null;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            recieverId = parameters.GetValue<int>("RecieverId");
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
    }
}
