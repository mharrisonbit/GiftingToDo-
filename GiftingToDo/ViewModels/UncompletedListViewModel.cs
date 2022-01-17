using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace GiftingToDo.ViewModels
{
    public class UncompletedListViewModel : ViewModelBase, IPageLifecycleAware
    {
        public IGiftingService giftService { get; }
        public ICypher cypher { get; }
        public IShare share { get; }

        public DelegateCommand<Receiver> CurrentSelectedPersonCmd { get; private set; }
        public DelegateCommand<Gift> ItemPurchasedCheck { get; private set; }
        public DelegateCommand AddPersonBtn { get; private set; }
        public DelegateCommand AddGiftCmd { get; private set; }
        public DelegateCommand ShareItemCmd { get; private set; }
        public DelegateCommand RemovePersonCmd { get; private set; }

        public UncompletedListViewModel(INavigationService navigationService, IErrorHandler errorHandler, IGiftingService giftService, ICypher cypher, IShare share) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
            this.cypher = cypher;
            this.share = share;

            CurrentSelectedPersonCmd = new DelegateCommand<Receiver>((x)=> UpdateBottomListWithInfo(x));
            ItemPurchasedCheck = new DelegateCommand<Gift>(async (x)=> await SetItemToPurchased(x));
            AddPersonBtn = new DelegateCommand(async () => await AddPerson());
            AddGiftCmd = new DelegateCommand(async () => await AddGiftToReciever());
            ShareItemCmd = new DelegateCommand(async () => await ShareRecieverAsync());
            RemovePersonCmd = new DelegateCommand(async () => await DeleteReciever());

            ListOfGifts = new ObservableCollection<Gift>();
            RecieverBtnsVisable = false;
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

        bool _RecieverBtnsVisable;
        public bool RecieverBtnsVisable
        {
            get { return _RecieverBtnsVisable; }
            set { SetProperty(ref _RecieverBtnsVisable, value); }
        }

        private Receiver selectedReceiver { get; set; }

        private void UpdateBottomListWithInfo(Receiver receiver)
        {
            try
            {
                selectedReceiver = receiver;
                ListOfGifts.Clear();
                ListOfGifts = new ObservableCollection<Gift>(receiver.Gifts);
                RecieverBtnsVisable = true;
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
            ListOfGifts.Clear();
            RecieverBtnsVisable = false;
        }

        private async Task SetItemToPurchased(Gift gift)
        {
            if (gift != null)
            {
                var updated = await this.giftService.UpdateGiftInfo(gift);
                if (updated)
                {
                    for (int i = 0; i < ListOfNames.Count; i++)
                    {
                        if (ListOfNames[i].Id == gift.ReceiverId)
                        {
                            for (int j = 0; j < ListOfNames[i].Gifts.Count; j++)
                            {
                                if (ListOfNames[i].Gifts[j].Id == gift.Id)
                                {
                                    ListOfNames[i].Gifts[j].ItemPurchased = ListOfNames[i].Gifts[j].ItemPurchased;
                                    ListOfGifts[j].ItemPurchased = ListOfNames[i].Gifts[j].ItemPurchased;
                                }
                            }
                        }
                    }

                    var person = ListOfNames.Where(x => x.Id == gift.ReceiverId).FirstOrDefault();
                    var giftCount = 0;
                    foreach (var item in person.Gifts)
                    {
                        if (item.ItemPurchased)
                        {
                            giftCount++;
                        }
                    }
                    

                    if (giftCount == person.Gifts.Count)
                    {
                        await GetAllRecievers();
                        ListOfGifts.Clear();
                    }
                }
            }

        }

        private async Task AddGiftToReciever()
        {
            var navParameters = new NavigationParameters
            {
                { "RecieverId", selectedReceiver.Id }
            };
            await this.NavigationService.NavigateAsync("AddGiftView", navParameters);
        }

        private async Task ShareRecieverAsync()
        {
            var itemToShare = await this.giftService.GetRecieverAsync(selectedReceiver.Id);
            var tempTextForExport = await this.giftService.CreateJsonForExport(itemToShare);
            var textForExport = this.cypher.Base64Encode(tempTextForExport);

            var fn = $"Add_{selectedReceiver.FirstName}_{selectedReceiver.LastName}_And_My_Gifts.txt";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            File.WriteAllText(file, textForExport);

            await this.share.RequestAsync(new ShareFileRequest
            {
                Title = $"Share {selectedReceiver.FirstName} with others",
                File = new ShareFile(file)
            });
        }

        private async Task DeleteReciever()
        {
            await this.giftService.SetRecieverAsDeleted(selectedReceiver.Id);
            await GetAllRecievers();
        }

        private async Task AddPerson()
        {
            await this.NavigationService.NavigateAsync("AddPersonView");
        }

        public void OnAppearing()
        {
            GetAllRecievers();
            RecieverBtnsVisable = false;
        }

        public void OnDisappearing()
        {
        }
    }
}
