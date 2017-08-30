using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

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

        public static string CurrentLocation
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentLocation), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(CurrentLocation), value);
        }
    }
}
