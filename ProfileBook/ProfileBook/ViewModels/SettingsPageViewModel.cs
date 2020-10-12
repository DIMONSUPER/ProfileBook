using Prism.Navigation;
using ProfileBook.Helpers;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public ICommand DateLabelClickCommand => new Command(DateLabelClick);
        public ICommand NameLabelClickCommand => new Command(NameLabelClick);
        public ICommand NickNameLabelClickCommand => new Command(NickNameLabelClick);

        public SettingsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Settings";
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
