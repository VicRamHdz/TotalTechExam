using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Prism.Navigation;
using Prism.Services;
using TotalTech.Framework;
using TotalTech.Models;
using TotalTech.Services;
using TotalTech.Storage;
using Xamarin.Forms;

namespace TotalTech.ViewModels
{
    public class PersonPageViewModel : BaseViewModel
    {
        private ObservableCollection<Persons> _Persons;
        public ObservableCollection<Persons> Persons
        {
            get => _Persons;
            set => SetProperty(ref _Persons, value);
        }

        PersonService service;

        public PersonPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigation = navigationService;
            _dialogService = dialogService;
            service = new PersonService();
        }

        public async Task LoadPersons()
        {
            IsBusy = true;
            try
            {
                BusyMessage = "Working...";
                var response = await service.Get(5);
                if (response.IsSuccess)
                {
                    Persons = new ObservableCollection<Persons>(response.Data.results);
                    CalculateRating(Persons);
                    await CacheHelper.UpdateCache("ContactsInfo", response.Data.results);
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

        public void CalculateRating(ObservableCollection<Persons> persons)
        {
            decimal rating  = 0;
            Random random = new Random();

            foreach (var item in persons)
            {
                for (int i = 0; i < 10; i++)
                {
                    rating = rating + random.Next(0, 100);
                }
                item.rating = rating / 10;
            }
        }
    }
}
