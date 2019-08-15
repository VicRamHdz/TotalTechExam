using System;
using Xamarin.Essentials;

namespace TotalTech.Storage
{
    public static class Settings
    {
        public static string Token
        {
            get
            {
                return Preferences.Get("Token", "");
            }
            set
            {
                Preferences.Set("Token", value);
            }
        }
    }
}
