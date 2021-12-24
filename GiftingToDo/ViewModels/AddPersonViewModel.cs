using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class AddPersonViewModel : ViewModelBase
    {
        public DelegateCommand AddPersonToDb { get; private set; }

        public IGiftingService giftService { get; private set; }

        public AddPersonViewModel(INavigationService navigationService, IGiftingService giftService, IErrorHandler errorHandler) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
            AddPersonToDb = new DelegateCommand(async ()=> await AddPerson());

            PersonToAdd = new Receiver();
        }

        Receiver _PersonToAdd;
        public Receiver PersonToAdd
        {
            get { return _PersonToAdd; }
            set { SetProperty(ref _PersonToAdd, value); }
        }

        private async Task AddPerson()
        {
            try
            {
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
    }
}
