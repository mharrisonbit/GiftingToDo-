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
    public class MainPageViewModel : ViewModelBase, IPageLifecycleAware
    {
        public IGiftingService giftService { get; }

        public DelegateCommand AddPersonBtn { get; private set; }
        public DelegateCommand RefreshListCmd { get; private set; }
        public DelegateCommand GetAllGifts { get; private set; }
        public DelegateCommand<object> AddGiftCmd { get; private set; }
        public DelegateCommand<object> RemovePersonCmd { get; private set; }
        public DelegateCommand<Gift> ItemPurchasedCheck { get; private set; }

        public MainPageViewModel(INavigationService navigationService, IGiftingService giftService, IErrorHandler errorHandler) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
            AddPersonBtn = new DelegateCommand(async ()=> await AddPerson());
            RefreshListCmd = new DelegateCommand(async ()=> await PopulateData());
            GetAllGifts = new DelegateCommand(async ()=> await RemoveAllGiftsFromDb());
            AddGiftCmd = new DelegateCommand<object>(async (x)=> await AddGiftToReciever(x));
            RemovePersonCmd = new DelegateCommand<object>(async (x)=> await DeleteReciever(x));
            ItemPurchasedCheck = new DelegateCommand<Gift>(async (x)=> await SetItemToPurchased(x));
        }

        string _FirstNameEntry;
        public string FirstNameEntry
        {
            get { return _FirstNameEntry; }
            set { SetProperty(ref _FirstNameEntry, value); }
        }

        ObservableCollection<Receiver> _ListOfRecivers;
        public ObservableCollection<Receiver> ListOfRecivers
        {
            get { return _ListOfRecivers; }
            set { SetProperty(ref _ListOfRecivers, value); }
        }

        private async Task PopulateData()
        {
            IsBusy = true;
            await GetAllRecievers();
            IsBusy = false;
        }

        private async Task AddPerson()
        {
            await this.NavigationService.NavigateAsync("AddPersonView");
        }

        async Task GetAllRecievers()
        {
            var tempList = await this.giftService.GetUncompeletedReciversAsync();
            var temp = new ObservableCollection<Receiver>(tempList);
            ListOfRecivers = temp;
        }


        private async Task AddGiftToReciever(object id)
        {
            var val = Convert.ToInt32(id);
            var navParameters = new NavigationParameters
            {
                { "RecieverId", val }
            };
            await this.NavigationService.NavigateAsync("AddGiftView", navParameters);
        }

        private async Task DeleteReciever(object reciever)
        {
            var val = Convert.ToInt32(reciever);
            await this.giftService.RemoveRecieverAsync(val);
            await PopulateData();
        }

        private async Task SetItemToPurchased(Gift gift)
        {
            if (gift != null)
            {
                var updated = await this.giftService.UpdateGiftInfo(gift);
                await GetAllRecievers();
            }
            
        }
        private async Task GetAllGiftsTest()
        {
            var answer = await this.giftService.GetAllGiftsInDataBase();
            Console.WriteLine(answer);
        }

        private async Task RemoveAllGiftsFromDb()
        {
            await this.giftService.RemoveAllGiftsFromDb();
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
