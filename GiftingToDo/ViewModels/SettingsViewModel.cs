using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.Models;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace GiftingToDo.ViewModels
{
    public class SettingsViewModel : ViewModelBase, IPageLifecycleAware
    {
        public IAppInfo appInfo { get; }
        public IGiftingService giftService { get; }

        public DelegateCommand RemoveAllPeopleBtn { get; private set; }

        public SettingsViewModel(INavigationService navigationService, IErrorHandler errorHandler, IAppInfo appInfo, IGiftingService giftService) : base(navigationService, errorHandler)
        {
            this.appInfo = appInfo;
            this.giftService = giftService;

            RemoveAllPeopleBtn = new DelegateCommand(async ()=> await RemoveAllPeopleCmd());
            VersionNumberTxt = this.appInfo.VersionString;
        }

        string _VersionNumberTxt;
        public string VersionNumberTxt
        {
            get { return _VersionNumberTxt; }
            set { SetProperty(ref _VersionNumberTxt, value); }
        }

        ObservableCollection<Receiver> _ListOfRemovedRecievers;
        public ObservableCollection<Receiver> ListOfRemovedRecievers
        {
            get { return _ListOfRemovedRecievers; }
            set { SetProperty(ref _ListOfRemovedRecievers, value); }
        }

        private async Task RemoveAllPeopleCmd()
        {
            await this.giftService.RemoveAllGiftsFromDb();
        }

        private async Task PopulateListData()
        {
            ListOfRemovedRecievers = new ObservableCollection<Receiver>(await this.giftService.GetAllDeletedReciversAsync());
        }

        public void OnAppearing()
        {
            PopulateListData();
        }

        public void OnDisappearing()
        {
        }
    }
}
