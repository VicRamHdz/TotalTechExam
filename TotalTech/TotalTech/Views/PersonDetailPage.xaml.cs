using System;
using System.Collections.Generic;
using TotalTech.Models;
using TotalTech.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TotalTech.Views
{
    public partial class PersonDetailPage : ContentPage
    {
        PersonDetailPageViewModel vm;

        public PersonDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string, Persons>(this, "LoadMap", async (text, input) =>
            {
                if (!string.IsNullOrEmpty(input.location.coordinates.longitude) && !string.IsNullOrEmpty(input.location.coordinates.latitude))
                {
                    MapView.MoveToRegion(
                   MapSpan.FromCenterAndRadius(
                    new Position(double.Parse(input.location.coordinates.latitude), double.Parse(input.location.coordinates.longitude)), Distance.FromMiles(1)));
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<string, Persons>(this, "LoadMap");
        }
    }
}
