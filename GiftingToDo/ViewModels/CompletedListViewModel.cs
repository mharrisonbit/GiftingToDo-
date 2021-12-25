using System;
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
    public class CompletedListViewModel : ViewModelBase, IPageLifecycleAware
    {

        public IGiftingService giftingService { get; }

        public DelegateCommand RefreshListCmd { get; private set; }
        public DelegateCommand<Gift> ItemPurchasedCheck { get; private set; }
        public DelegateCommand<Receiver> RemovePersonCmd { get; private set; }

        public CompletedListViewModel(INavigationService navigationService, IErrorHandler errorHandler, IGiftingService giftingService) : base(navigationService, errorHandler)
        {
            this.giftingService = giftingService;
            RefreshListCmd = new DelegateCommand(async () => await PopulateData());
            ItemPurchasedCheck = new DelegateCommand<Gift>(async (x) => await SetItemToPurchased(x));
            RemovePersonCmd = new DelegateCommand<Receiver>(async (x) => await DeleteReciever(x));
        }

        ObservableCollection<Receiver> _FinishedList;
        public ObservableCollection<Receiver> FinishedList
        {
            get { return _FinishedList; }
            set { SetProperty(ref _FinishedList, value); }
        }

        private async Task PopulateData()
        {
            IsBusy = true;
            var tempList = await this.giftingService.GetCompeletedReciversAsync();
            FinishedList = new ObservableCollection<Receiver>(tempList);
            IsBusy = false;
        }

        private async Task SetItemToPurchased(Gift gift)
        {
            if (gift != null)
            {
                var updated = await this.giftingService.UpdateGiftInfo(gift);
                await PopulateData();
            }

        }

        private async Task DeleteReciever(Receiver reciever)
        {
            await this.giftingService.RemoveRecieverAsync(reciever.Id);
            await PopulateData();
        }

        public void OnAppearing()
        {
            PopulateData();
        }

        public void OnDisappearing()
        {

        }
    }
}
