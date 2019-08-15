using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using TotalTech.Models;
using Xamarin.Forms;

namespace TotalTech.Framework
{
    public abstract class BaseViewModel : BindableBase, INavigationAware
    {
        internal INavigationService _navigation { get; set; }
        internal IPageDialogService _dialogService { get; set; }

        private string _busyMessage = "";

        private bool _isBusy;
        internal bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                try
                {
                    if (DisplayLoading)
                    {
                        if (IsBusy)
                        {
                            UserDialogs.Instance.ShowLoading(BusyMessage, Device.RuntimePlatform == Device.iOS ? MaskType.Black : MaskType.Clear);
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            _busyMessage = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Can not display or hide Loading dialog, Error {ex.Message} Stack: {ex.StackTrace}");
                }
                RaisePropertyChanged();
            }
        }

        internal string BusyMessage
        {
            get
            {
                return _busyMessage;
            }
            set
            {
                _busyMessage = value;
                if (IsBusy && DisplayLoading)
                {
                    UserDialogs.Instance.ShowLoading(BusyMessage, Device.RuntimePlatform == Device.iOS ? MaskType.Black : MaskType.Clear);
                }
            }
        }

        internal bool DisplayLoading
        {
            get; set;
        }

        public BaseViewModel()
        {
            DisplayLoading = true;
        }


        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public async Task DisplayMessage(string title, string message)
        {
            if (IsBusy)
                IsBusy = false;
            await _dialogService?.DisplayAlertAsync(title, message, "OK");
        }

        public async Task DisplayError(Exception ex)
        {
            if (IsBusy)
                IsBusy = false;

            //Showing error to console
            Console.WriteLine(ex.ToString());

            //Display general error
            await _dialogService?.DisplayAlertAsync("Error", "Something wrong occurred", "OK");
        }

        public async Task DisplayApiMessage<T>(ResponseResult<T> response)
        {
            if (IsBusy)
                IsBusy = false;

            if (response.StatusCode == 400)
            {
                //Display business error message
                await _dialogService?.DisplayAlertAsync("Info", response.Message, "OK");
            }
            else if (response.StatusCode == 500)
            {
                //Showing error to console
                Console.WriteLine(response.Message);

                //Display general error
                await _dialogService?.DisplayAlertAsync("Error", "Something wrong occurred", "OK");
            }
        }
    }
}
