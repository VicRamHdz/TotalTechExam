using System;
using Prism.Navigation;
using Prism.Services;
using TotalTech.Framework;
using TotalTech.Models;
using Xamarin.Forms;

namespace TotalTech.ViewModels
{
    public class PersonDetailPageViewModel : BaseViewModel
    {
        private Persons _Person;
        public Persons Person
        {
            get => _Person;
            set => SetProperty(ref _Person, value);
        }

        public PersonDetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigation = navigationService;
            _dialogService = dialogService;
        }

        public override async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("person"))
            {
                Person = parameters["person"] as Persons;

                MessagingCenter.Send<string, Persons>("LoadMap", "LoadMap", Person);
            }
        }
    }
}
