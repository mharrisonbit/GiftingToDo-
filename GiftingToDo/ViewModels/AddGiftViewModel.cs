using System.Collections.ObjectModel;
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
            GiftsToBeAdded = new ObservableCollection<Gift>();
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

        ObservableCollection<Gift> _GiftsToBeAdded;
        public ObservableCollection<Gift> GiftsToBeAdded
        {
            get { return _GiftsToBeAdded; }
            set { SetProperty(ref _GiftsToBeAdded, value); }
        }

        private async Task AddGiftToUser()
        {
            try
            {
                giftToAdd.ItemDescription = ItemDescription;
                giftToAdd.Price = ConvertStringToDouble(ItemPrice);

                var wasAdded = await this.giftingService.AddGiftToUserAsync(recieverId, giftToAdd);
                if (wasAdded)
                {
                    GiftsToBeAdded.Add(giftToAdd);
                    ItemPrice = "";
                    ItemDescription = "";
                    giftToAdd.ItemDescription = "";
                    giftToAdd.Price = 0.0;
                }
            }
            catch (System.Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
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
