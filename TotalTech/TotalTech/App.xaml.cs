using System;
using Akavache;
using FFImageLoading.Svg.Forms;
using Prism.Unity;
using TotalTech.Controls;
using TotalTech.Storage;
using TotalTech.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TotalTech
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer plataInitializer = null) : base(plataInitializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            var ignore = new SvgCachedImage();
            //IsFirstLoad = true;
            if (string.IsNullOrEmpty(Settings.Token))
                NavigationService.NavigateAsync("LoginPage");
            else
                NavigationService.NavigateAsync("RootPage/PersonPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<RootPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<PersonPage>();
            Container.RegisterTypeForNavigation<PersonDetailPage>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            BlobCache.ApplicationName = "ContactsBlob";
            BlobCache.EnsureInitialized();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            BlobCache.Shutdown().Wait();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            BlobCache.EnsureInitialized();
        }
    }
}