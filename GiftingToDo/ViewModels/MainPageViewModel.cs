using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public IGiftingService giftService { get; }
        public IErrorHandler errorHandler { get; }

        public DelegateCommand AddPersonBtn { get; private set; }
        public DelegateCommand RefreshListCmd { get; private set; }

        public MainPageViewModel(INavigationService navigationService, IGiftingService giftService, IErrorHandler errorHandler) : base(navigationService)
        {
            this.giftService = giftService;
            this.errorHandler = errorHandler; 
            AddPersonBtn = new DelegateCommand(async ()=> await AddPerson());
            RefreshListCmd = new DelegateCommand(async ()=> await PopulateData());

            PopulateData();
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
            try
            {
                var person = new Receiver();
                person.FirstName = FirstNameEntry;
                var temp = new Gift();
                temp.ItemDescription = "this is a thing";
                temp.PaperWrappedIn = "Purple";
                temp.Price = 10.99;
                person.Gifts = new List<Gift>
                {
                    temp
                };

                await this.giftService.AddReceiverAsync(person);
                ListOfRecivers.Add(person);
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        async Task GetAllRecievers()
        {
            var tempList = await this.giftService.GetAllReciversAsync();
            var temp = new ObservableCollection<Receiver>(tempList);
            ListOfRecivers = temp;
        }
    }
}
