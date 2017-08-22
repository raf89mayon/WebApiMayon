using HomeSnailHome.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Windows.Input;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using Windows.UI.Popups;
using System.Collections.ObjectModel;
using HomeSnailHome.Views;
using AppLabo5.Exceptions;
using System.Threading.Tasks;
using HomeSnailHome.Exceptions;

namespace HomeSnailHome.ViewModel
{
    public class NewHousingViewModel : ViewModelBase
    {
        private HousingPost _newHousing;
        public HousingPost NewHousing
        {
            get
            {
                return _newHousing;
            }
            set
            {
                if (value != _newHousing)
                {
                    _newHousing = value;
                    OnNotifyPropertyChanged("NewHousing");
                }
            }
        }

        public ObservableCollection<Bed> _bedTypeCollection;
        public ObservableCollection<Bed> BedTypeCollection
        {
            get
            {
                return _bedTypeCollection;
            }
            set
            {
                _bedTypeCollection = value;
                RaisePropertyChanged("BedTypeCollection");
            }
        }

        public ObservableCollection<Locality> _localitiesCollection;
        public ObservableCollection<Locality> LocalitiesCollection
        {
            get
            {
                return _localitiesCollection;
            }
            set
            {
                if (value != _localitiesCollection)
                {
                    _localitiesCollection = value;
                    RaisePropertyChanged("LocalitiesCollection");
                }
            }
        }

        private Bed _selectedBedType;
        public Bed SelectedBedType
        {
            get
            {
                return _selectedBedType;
            }
            set
            {
                _selectedBedType = value;
                RaisePropertyChanged("SelectedBedType");
            }
        }

        private Locality _selectedLocality;
        public Locality SelectedLocality
        {
            get
            {
                return _selectedLocality;
            }
            set
            {
                _selectedLocality = value;
                RaisePropertyChanged("SelectedLocality");
            }
        }

        private String _description;
        public String Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                }
            }
        }


        private bool _wifiChecked;
        public bool WifiChecked
        {
            get { return _wifiChecked; }
            set
            {
                if (_wifiChecked != value)
                {
                    _wifiChecked = value;
                }
            }
        }

        private bool _officeChecked;
        public bool OfficeChecked
        {
            get { return _officeChecked; }
            set
            {
                if (_officeChecked != value)
                {
                    _officeChecked = value;
                }
            }
        }

        private bool _kitchenChecked;
        public bool KitchenChecked
        {
            get { return _kitchenChecked; }
            set
            {
                if (_kitchenChecked != value)
                {
                    _kitchenChecked = value;
                }
            }
        }

        private bool _toiletChecked;
        public bool ToiletChecked
        {
            get { return _toiletChecked; }
            set
            {
                if (_toiletChecked != value)
                {
                    _toiletChecked = value;
                }
            }
        }

        private bool _showerChecked;
        public bool ShowerChecked
        {
            get { return _showerChecked; }
            set
            {
                if (_showerChecked != value)
                {
                    _showerChecked = value;
                }
            }
        }

        private DateTimeOffset _startDay;
        public DateTimeOffset StartDay
        {
            get
            {
                return _startDay;
            }
            set
            {
                _startDay = value;
                OnNotifyPropertyChanged("StartDay");
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
                OnNotifyPropertyChanged("StartTime");
            }
        }

        private DateTimeOffset _endDay;
        public DateTimeOffset EndDay
        {
            get
            {
                return _endDay;
            }
            set
            {
                _endDay = value;
                OnNotifyPropertyChanged("EndDay");
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                OnNotifyPropertyChanged("EndTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public String strPostBox { get; set; }
        public String strZipCode { get; set; }
        
        public NewHousingViewModel(INavigationService navigationService)
        {
            NewHousing = new HousingPost
            {
//                HostID = ViewModelLocator.ConnectedUser.ID,

                Number = null,
                PostBox = 0,
                Street = null,
                ZipCode = 0,
                City = null,

                Wifi = false,
                Kitchen = false,
                Office = false,
                Shower = false,
                Toilet = false,

                SpaceLocalization = null,
                Description = null,
                Pictures = null,

                StartDate = DateTime.Now,
                EndDate = DateTime.Now,

                BedType = 1
            };
            StartDay = new DateTimeOffset(DateTime.Now);
            EndDay = new DateTimeOffset(DateTime.Now);
            StartTime = new TimeSpan(14, 00, 00);
            EndTime = new TimeSpan(14, 00, 00);
            _bedTypeCollection = ViewModelLocator.BedsList;
            _localitiesCollection = ViewModelLocator.LocalitiesList;
            _selectedBedType = _bedTypeCollection[0];
            _selectedLocality = _localitiesCollection[0];
        }

        private ICommand _goBackToHomeCommand;
        public ICommand GoBackToHomeCommand
        {
            get
            {
                if (_goBackToHomeCommand == null)
                {
                    _goBackToHomeCommand = new RelayCommand(() => CancelConfirmation());
                }

                return _goBackToHomeCommand;
            }
        }

        private async void CancelConfirmation()
        {
            MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir annuler la création d'un compte utilisateur ?", "Annulation");

            UICommand okBtn = new UICommand("Oui");
            okBtn.Invoked = GoBack;
            msgDialog.Commands.Add(okBtn);

            UICommand cancelBtn = new UICommand("Non");
            msgDialog.Commands.Add(cancelBtn);

            await msgDialog.ShowAsync();
        }

        private void GoBack(IUICommand command)
        {
            ShellViewModel.MyFrame.GoBack();
        }

        private ICommand _goNextCommand;
        public ICommand GoNextCommand
        {
            get
            {
                if (_goNextCommand == null)
                {
                    _goNextCommand = new RelayCommand(() => AddConfirmation());
                }

                return _goNextCommand;
            }
        }

        private Boolean ValidateHousingEntries()
        {
            

            return true;
        }

        private async void AddConfirmation()
        {
            try
            {
                int numPB = 0;

                NewHousing.Kitchen = KitchenChecked;
                NewHousing.Wifi = WifiChecked;
                NewHousing.Office = OfficeChecked;
                NewHousing.Shower = ShowerChecked;
                NewHousing.Toilet = ToiletChecked;

                NewHousing.StartDate = new DateTime(StartDay.Year, StartDay.Month, StartDay.Day, StartTime.Hours, StartTime.Minutes, 0);
                NewHousing.EndDate = new DateTime(EndDay.Year, EndDay.Month, EndDay.Day, EndTime.Hours, EndTime.Minutes, 0);

                if (StartDay > EndDay)
                {
                    throw new AddEditHousingException(1);
                }
                if (StartDay == EndDay)
                {
                    throw new AddEditHousingException(2);
                }
                if (SelectedBedType.Code <= 0)
                {
                    throw new AddEditHousingException(3);
                }
                NewHousing.BedType = SelectedBedType.Code;
                if (!String.IsNullOrWhiteSpace(Description))
                {
                    NewHousing.Description = Description;
                }
                else
                {
                    NewHousing.Description = null;
                }
                if (String.IsNullOrWhiteSpace(NewHousing.Number))
                {
                    throw new AddressException(1);
                }
                if (!String.IsNullOrWhiteSpace(strPostBox))
                {
                    bool parsed = Int32.TryParse(strPostBox, out numPB);
                    if (!parsed)
                    {
                        throw new AddressException(2);
                    }
                }
                if (String.IsNullOrWhiteSpace(NewHousing.Street))
                {
                    throw new AddressException(5);
                }
                if (SelectedLocality != null)
                {
                    NewHousing.ZipCode = SelectedLocality.Zip;
                    NewHousing.City = SelectedLocality.Name;
                }
                else
                {
                    throw new AddressException(9);
                }
                if (String.IsNullOrWhiteSpace(NewHousing.Pictures))
                {
                    NewHousing.Pictures = null;
                }
                NewHousing.PostBox = numPB;

                MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir poursuivre la création d'une annonce de logement ?", "Confirmation");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = GoNext;
                msgDialog.Commands.Add(okBtn);

                UICommand cancelBtn = new UICommand("Non");
                msgDialog.Commands.Add(cancelBtn);

                await msgDialog.ShowAsync();
            }
            catch (AddEditHousingException addUserEx)
            {
                await DisplayException(addUserEx.ErrorMessage);
            }
            catch (AddressException addressEx)
            {
                await DisplayException(addressEx.ErrorMessage);
            }
        }

        private async Task DisplayException(string message)
        {
            await new MessageDialog(message, "Erreur").ShowAsync();
        }

        private async void GoNext(IUICommand command)
        {
            await new HousingService().AddOneHousing(NewHousing);
            ShellViewModel.MyFrame.Navigate(typeof(HousingPage));
        }

        private bool _progressIsIndeterminate;
        public bool ProgressIsIndeterminate
        {
            get
            {
                return _progressIsIndeterminate;
            }
            set
            {
                if (_progressIsIndeterminate != value)
                {
                    _progressIsIndeterminate = value;
                    RaisePropertyChanged("ProgressIsIndeterminate");
                }
            }
        }

        private double _progressBarOpacity;
        public double ProgressBarOpacity
        {
            get
            {
                return _progressBarOpacity;
            }
            set
            {
                if (_progressBarOpacity != value)
                {
                    _progressBarOpacity = value;
                    RaisePropertyChanged("ProgressBarOpacity");
                }
            }
        }

        public void ProgressBarActivity(bool active)
        {
            if (active)
            {
                ProgressIsIndeterminate = true;
                ProgressBarOpacity = 1;
            }
            else
            {
                ProgressIsIndeterminate = false;
                ProgressBarOpacity = 0;
            }
        }
    }
}
