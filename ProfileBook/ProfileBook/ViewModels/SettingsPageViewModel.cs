using Prism.Navigation;
using ProfileBook.Helpers;
using ProfileBook.Resources;
using ProfileBook.Themes;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public ICommand DateLabelClickCommand => new Command(DateLabelClick);
        public ICommand NameLabelClickCommand => new Command(NameLabelClick);
        public ICommand NickNameLabelClickCommand => new Command(NickNameLabelClick);
        public ICommand CheckBoxClickCommand => new Command(CheckBoxClick);

        public SettingsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            IsDarkTheme = Settings.RememberedCheckBox;
            if (!string.IsNullOrEmpty(Settings.RememberedLanguage))
            {
                SelectedItem = Settings.RememberedLanguage;
            }
            else
            {
                SelectedItem = "English";
            }

            if (!string.IsNullOrEmpty(Settings.RememberedRadioButton))
            {
                switch (Settings.RememberedRadioButton)
                {
                    case nameof(SortByDate):
                        SortByDate = true;
                        break;
                    case nameof(SortByName):
                        SortByName = true;
                        break;
                    case nameof(SortByNickName):
                        SortByNickName = true;
                        break;
                    default:
                        SortByDate = true;
                        break;
                }
            }
            else
            {
                SortByDate = true;
            }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                if (_selectedItem != null)
                {
                    var language = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().First(element => element.EnglishName.Contains(_selectedItem.ToString()));
                    Thread.CurrentThread.CurrentUICulture = language;
                    AppResources.Culture = language;

                    if (_selectedItem != Settings.RememberedLanguage)
                    {
                        Settings.RememberedLanguage = _selectedItem;
                        NavigationService.NavigateAsync("/NavigationPage/MainListPage");
                    }
                }
            }
        }

        private bool isDarkTheme;
        public bool IsDarkTheme
        {
            get { return isDarkTheme; }
            set
            {
                SetProperty(ref isDarkTheme, value);
                Settings.RememberedCheckBox = value;

                ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
                if (mergedDictionaries != null)
                {
                    mergedDictionaries.Clear();
                    if (isDarkTheme == true)
                    {
                        mergedDictionaries.Add(new DarkTheme());
                    }
                    else
                    {
                        mergedDictionaries.Add(new LightTheme());
                    }
                }
            }
        }

        private bool sortByDate;
        public bool SortByDate
        {
            get { return sortByDate; }
            set
            {
                SetProperty(ref sortByDate, value);
                if (value == true)
                    Settings.RememberedRadioButton = nameof(SortByDate);
            }
        }

        private bool sortByName;
        public bool SortByName
        {
            get { return sortByName; }
            set
            {
                SetProperty(ref sortByName, value);
                if (value == true)
                    Settings.RememberedRadioButton = nameof(SortByName);
            }
        }

        private bool sortByNickName;
        public bool SortByNickName
        {
            get { return sortByNickName; }
            set
            {
                SetProperty(ref sortByNickName, value);
                if (value == true)
                    Settings.RememberedRadioButton = nameof(SortByNickName);
            }
        }

        private void CheckBoxClick()
        {
            IsDarkTheme = IsDarkTheme != true;
        }

        private void DateLabelClick()
        {
            SortByDate = true;
        }

        private void NameLabelClick()
        {
            SortByName = true;
        }

        private void NickNameLabelClick()
        {
            SortByNickName = true;
        }
    }
}
