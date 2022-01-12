using System;
using System.IO;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;

namespace GiftingToDo.ViewModels
{
    public class AddPersonViewModel : ViewModelBase
    {
        public IGiftingService giftService { get; private set; }
        public ICypher cypher { get; }

        public DelegateCommand AddPersonToDb { get; private set; }
        public DelegateCommand ImportPersonToDbCmd { get; private set; }

        public AddPersonViewModel(INavigationService navigationService, IGiftingService giftService, IErrorHandler errorHandler, ICypher cypher) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
            this.cypher = cypher;

            AddPersonToDb = new DelegateCommand(async ()=> await AddPerson());
            ImportPersonToDbCmd = new DelegateCommand(async ()=> await ImportNewReciever());

            PersonToAdd = new Receiver();
            DatePickerMaxDate = DateTime.Now;
            DatePickerMinDate = DateTime.Now.AddYears(-100);
            AmountToSpend = new double();
        }

        Receiver _PersonToAdd;
        public Receiver PersonToAdd
        {
            get { return _PersonToAdd; }
            set { SetProperty(ref _PersonToAdd, value); }
        }

        DateTime _DatePickerMaxDate;
        public DateTime DatePickerMaxDate
        {
            get { return _DatePickerMaxDate; }
            set { SetProperty(ref _DatePickerMaxDate, value); }
        }

        DateTime _DatePickerMinDate;
        public DateTime DatePickerMinDate
        {
            get { return _DatePickerMinDate; }
            set { SetProperty(ref _DatePickerMinDate, value); }
        }

        double _AmountToSpend;
        public double AmountToSpend
        {
            get { return _AmountToSpend; }
            set { SetProperty(ref _AmountToSpend, value); }
        }

        private async Task AddPerson()
        {
            try
            {
                PersonToAdd.SpendingLimit = AmountToSpend;
                var personAdded = await this.giftService.AddReceiverAsync(PersonToAdd);
                if (personAdded)
                {
                    await this.NavigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }

        private async Task ImportNewReciever()
        {
            var result = await FilePicker.PickAsync();
            if (result != null)
            {
                var tempInfoToAdd = File.ReadAllText(result.FullPath);
                var infoToAdd = this.cypher.Base64Decode(tempInfoToAdd);
                var answer = await this.giftService.ImportNewReciever(infoToAdd);
                if (answer)
                {
                    await this.NavigationService.GoBackAsync();
                }
            }

        }
    }
}
