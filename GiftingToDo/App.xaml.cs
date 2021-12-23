using GiftingToDo.Helpers;
using GiftingToDo.Interfaces.Implementations;
using GiftingToDo.Interfaces.Interfaces;
using GiftingToDo.ViewModels;
using GiftingToDo.Views;
using Prism.Ioc;
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

            var result = await NavigationService.NavigateAsync("NavigationPage/MainPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            //this is the interfaces that will be used.
            containerRegistry.RegisterSingleton<IErrorHandler, ErrorHandler>();
            containerRegistry.RegisterSingleton<IGiftingService, GiftingService>();
        }
    }
}
