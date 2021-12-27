using System;
using GiftingToDo.Helpers;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class TabbedViewModel : ViewModelBase
    {
        public TabbedViewModel(INavigationService navigationService, IErrorHandler errorHandler) : base(navigationService, errorHandler)
        {
        }
    }
}
