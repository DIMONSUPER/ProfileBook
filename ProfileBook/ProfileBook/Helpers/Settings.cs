using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ProfileBook.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string RememberedLoginSettingsKey = "remembered_login_key";
        private const string RememberedRadioButtonSettingsKey = "remembered_radiobutton_key";
        private const string RememberedCheckBoxSettingsKey = "remembered_checkbox_key";
        private const string RememberedLanguageKey = "remembered_language_key";
        private static readonly string SettingsDefault = string.Empty;
        private static readonly bool BoolSettingsDefault = false;

        #endregion

        public static bool RememberedCheckBox
        {
            get
            {
                return AppSettings.GetValueOrDefault(RememberedCheckBoxSettingsKey, BoolSettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RememberedCheckBoxSettingsKey, value);
            }
        }
        public static string RememberedLanguage
        {
            get
            {
                return AppSettings.GetValueOrDefault(RememberedLanguageKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RememberedLanguageKey, value);
            }
        }
        public static string RememberedRadioButton
        {
            get
            {
                return AppSettings.GetValueOrDefault(RememberedRadioButtonSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RememberedRadioButtonSettingsKey, value);
            }
        }

        public static string RememberedLogin
        {
            get
            {
                return AppSettings.GetValueOrDefault(RememberedLoginSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RememberedLoginSettingsKey, value);
            }
        }
    }
}
