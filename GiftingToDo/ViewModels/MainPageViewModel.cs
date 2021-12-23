using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public IGiftingService giftService { get; }
        public DelegateCommand AddPersonBtn { get; private set; }

        public MainPageViewModel(INavigationService navigationService, IGiftingService giftService) : base(navigationService)
        {
            this.giftService = giftService;
            AddPersonBtn = new DelegateCommand(async ()=> await AddPerson());

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
            await GetAllRecievers();
        }

        private async Task AddPerson()
        {
            var person = new Receiver();
            person.FirstName = FirstNameEntry;
            await this.giftService.AddReceiverAsync(person);
            ListOfRecivers.Add(person);
        }

        async Task GetAllRecievers()
        {
            var tempList = await this.giftService.GetAllReciversAsync();
            var temp = new ObservableCollection<Receiver>(tempList);
            ListOfRecivers = temp;
        }
    }
}
