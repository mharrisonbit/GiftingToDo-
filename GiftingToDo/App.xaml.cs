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
            CheckCurrentDb();

            var result = await NavigationService.NavigateAsync("NavigationPage/TabbedView?selectedTab=MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedView, TabbedViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPersonView, AddPersonViewModel>();
            containerRegistry.RegisterForNavigation<AddGiftView, AddGiftViewModel>();
            containerRegistry.RegisterForNavigation<CompletedListView, CompletedListViewModel>();

            //this is the interfaces that will be used.
            containerRegistry.RegisterSingleton<IErrorHandler, ErrorHandler>();
            containerRegistry.RegisterSingleton<IGiftingService, GiftingService>();
            containerRegistry.RegisterSingleton<IShare, ShareImplementation>();
        }

        /// <summary>
        /// this is going to run and double check to make sure that the DB is running the proper version and that the items are properly updated.
        /// </summary>
        private void CheckCurrentDb()
        {
            double dbVersion = Preferences.Get("currentDbVersion", Constants.DefaultDbVersion);
            if (dbVersion <= Constants.CurrentDBVersion)
            {
                var dbUpdate = new DbChangeScripts();
                dbUpdate.DbUpdate(dbVersion);
            }
        }

    }
}
