using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class UncompletedListViewModel : ViewModelBase, IPageLifecycleAware
    {
        public IGiftingService giftService { get; }

        public DelegateCommand<Receiver> CurrentSelectedPersonCmd { get; private set; }
        public DelegateCommand<Gift> ItemPurchasedCheck { get; private set; }

        public UncompletedListViewModel(INavigationService navigationService, IErrorHandler errorHandler, IGiftingService giftService) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
            CurrentSelectedPersonCmd = new DelegateCommand<Receiver>((x)=> UpdateBottomListWithInfo(x));
            ItemPurchasedCheck = new DelegateCommand<Gift>(async (x)=> await SetItemToPurchased(x));
            ListOfGifts = new ObservableCollection<Gift>();
        }

        ObservableCollection<Receiver> _ListOfNames;
        public ObservableCollection<Receiver> ListOfNames
        {
            get { return _ListOfNames; }
            set { SetProperty(ref _ListOfNames, value); }
        }

        ObservableCollection<Gift> _ListOfGifts;
        public ObservableCollection<Gift> ListOfGifts
        {
            get { return _ListOfGifts; }
            set { SetProperty(ref _ListOfGifts, value); }
        }

        private void UpdateBottomListWithInfo(Receiver receiver)
        {
            try
            {
                ListOfGifts.Clear();
                ListOfGifts = new ObservableCollection<Gift>(receiver.Gifts);
            }
            catch (System.Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        async Task GetAllRecievers()
        {
            var tempList = await this.giftService.GetUncompeletedReciversAsync();
            var temp = new ObservableCollection<Receiver>(tempList);
            ListOfNames = temp;
        }

        private async Task SetItemToPurchased(Gift gift)
        {
            if (gift != null)
            {
                var updated = await this.giftService.UpdateGiftInfo(gift);
                await GetAllRecievers();
                ListOfGifts.Clear();
            }

        }

        public void OnAppearing()
        {
            GetAllRecievers();
        }

        public void OnDisappearing()
        {
        }
    }
}
