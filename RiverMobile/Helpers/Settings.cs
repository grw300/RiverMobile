using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RiverMobile.Models;

namespace RiverMobile.Helpers
{
    public static class Settings
    {
        /// <summary>
        /// This is the Settings static class that can be used in your Core solution or in any
        /// of your client applications. All settings are laid out the same exact way with getters
        /// and setters. 
        /// </summary>
        static ISettings AppSettings => CrossSettings.Current;

        public static int CurrentLocation
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentLocation), -1);
            set => AppSettings.AddOrUpdateValue(nameof(CurrentLocation), value);
        }

        public static bool IsLoggedIn
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLoggedIn), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLoggedIn), value);
        }

        public static string UserId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }
    }
}
