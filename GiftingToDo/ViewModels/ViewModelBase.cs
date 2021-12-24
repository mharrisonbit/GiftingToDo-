using System;
using GiftingToDo.Helpers;
using Prism.Mvvm;
using Prism.Navigation;

namespace GiftingToDo.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        public IErrorHandler errorHandler { get; private set; }

        public ViewModelBase(INavigationService navigationService, IErrorHandler errorHandler)
        {
            NavigationService = navigationService;
            this.errorHandler = errorHandler;
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }

        /// <summary>
        /// this will convert a string to a double.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected double ConvertStringToDouble(string val)
        {
            double.TryParse(val, out var tempVal);
            return tempVal;
        }

        /// <summary>
        /// This will convert the string that is sent from double to a string.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected string ConvertFromDoubleToString(double val)
        {
            var tempVal = val.ToString();
            return tempVal;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}