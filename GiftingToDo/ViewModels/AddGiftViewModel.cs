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

        public Gift giftToAdd { get; private set; }
        public int recieverId { get; private set; }
        public IGiftingService giftingService { get; }

        public AddGiftViewModel(INavigationService navigationService, IErrorHandler errorHandler, IGiftingService giftingService) : base(navigationService, errorHandler)
        {
            this.giftingService = giftingService;
            AddGiftToRecieverBtn = new DelegateCommand(async ()=> await AddGiftToUser());
            giftToAdd = new Gift();
        }

        string _ItemPrice;
        public string ItemPrice
        {
            get { return _ItemPrice; }
            set { SetProperty(ref _ItemPrice, value); }
        }

        string _ItemDescription;
        public string ItemDescription
        {
            get { return _ItemDescription; }
            set { SetProperty(ref _ItemDescription, value); }
        }


        private async Task AddGiftToUser()
        {
            giftToAdd.ItemDescription = ItemDescription;
            giftToAdd.Price = ConvertStringToDouble(ItemPrice);

            var wasAdded = await this.giftingService.AddGiftToUserAsync(recieverId, giftToAdd);
            if (wasAdded)
            {
                ItemPrice = "";
                ItemDescription = "";
                giftToAdd.ItemDescription = "";
                giftToAdd.Price = 0.0;
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
