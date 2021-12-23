using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class AddPersonViewModel : ViewModelBase
    {
        public IGiftingService giftService { get; private set; }

        public AddPersonViewModel(INavigationService navigationService, IGiftingService giftService, IErrorHandler errorHandler) : base(navigationService, errorHandler)
        {
            this.giftService = giftService;
        }


        private async Task AddPerson()
        {
            try
            {
                var person = new Receiver();
                //person.FirstName = FirstNameEntry;
                var temp = new Gift();
                temp.ItemDescription = "this is a thing";
                temp.PaperWrappedIn = "Purple";
                temp.Price = 10.99;
                person.Gifts = new List<Gift>
                {
                    temp
                };

                await this.giftService.AddReceiverAsync(person);
            }
            catch (Exception ex)
            {
                this.errorHandler.PrintErrorMessage(ex);
            }
        }
    }
}
