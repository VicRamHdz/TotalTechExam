using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using TotalTech.Framework;
using TotalTech.Services;
using TotalTech.Views;
using Xamarin.Forms;

namespace TotalTech.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public Command LoginCommand { get; set; }

        private string _UserName;
        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
        }

        private string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }

        private UserService _userService;

        public LoginPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigation = navigationService;
            _dialogService = dialogService;
            _userService = new UserService();
            LoginCommand = new Command(async () => await OnLogin());
        }

        private async Task OnLogin()
        {
            IsBusy = true;
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    await DisplayMessage("Info", "Please fill out user and password");
                    return;
                }

                BusyMessage = "Working...";

                var response = await _userService.Login(UserName, Password);
                if (response.IsSuccess)
                {
                    Application.Current.MainPage = new NavigationPage(new PersonPage());
                }
                else
                {
                    await DisplayApiMessage(response);
                }
            }
            catch (Exception ex)
            {
                await DisplayError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
