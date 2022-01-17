using System;
using System.Threading.Tasks;
using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Implementations;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.ViewModels;
using GiftingToDo.Views;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace GiftingToDo
{
    public partial class App
    {
        public App()
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await CheckCurrentDb();
            Preferences.Set("WasPurchased", true);

            var result = await NavigationService.NavigateAsync("NavigationPage/TabbedView?selectedTab=UncompletedListView");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedView, TabbedViewModel>();
            //containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPersonView, AddPersonViewModel>();
            containerRegistry.RegisterForNavigation<AddGiftView, AddGiftViewModel>();
            containerRegistry.RegisterForNavigation<CompletedListView, CompletedListViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<UncompletedListView, UncompletedListViewModel>();

            //this is the interfaces that will be used.
            containerRegistry.RegisterSingleton<IErrorHandler, ErrorHandler>();
            containerRegistry.RegisterSingleton<IGiftingService, GiftingService>();
            containerRegistry.RegisterSingleton<IShare, ShareImplementation>();
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterSingleton<ICypher, Cypher>();
        }

        /// <summary>
        /// this is going to run and double check to make sure that the DB is running the proper version and that the items are properly updated.
        /// </summary>
        private async Task CheckCurrentDb()
        {
            double dbVersion = Preferences.Get("currentDbVersion", Constants.DefaultDbVersion);
            if (dbVersion < Constants.CurrentDBVersion)
            {
                var dbUpdate = new DbChangeScripts();
                await dbUpdate.DbUpdate(dbVersion);
            }
        }
    }
}
