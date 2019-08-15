using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using TotalTech.Models;
using TotalTech.Storage;
using TotalTech.ViewModels;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace TotalTech.Views
{
    public partial class PersonPage : ContentPage
    {
        PersonPageViewModel vm;

        public PersonPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = this.BindingContext as PersonPageViewModel;
        }

        protected override void OnAppearing()
        {
            var contacts = CacheHelper.Cache.GetObject<List<Persons>>("ContactsInfo").Catch(Observable.Return(default(List<Persons>))).Wait();

            if (contacts == null)
                vm?.LoadPersons();
            else
            {
                vm.Persons = new ObservableCollection<Persons>(contacts);
                vm.CalculateRating(vm.Persons);
            }
        }
    }
}