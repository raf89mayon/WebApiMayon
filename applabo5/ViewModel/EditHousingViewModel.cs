using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Views;
using HomeSnailHome.Exceptions;

namespace HomeSnailHome.ViewModel
{
    public class EditHousingViewModel : ViewModelBase
    {
        public int SelectedHousingID;
        public Frame MyFrame;

        private HousingPost _editedHousing;
        public HousingPost EditedHousing
        {
            get
            {
                return _editedHousing;
            }
            set
            {
                if (value != _editedHousing)
                {
                    _editedHousing = value;
                    OnNotifyPropertyChanged("EditedHousing");
                }
            }
        }

        public String strPostBox { get; set; }
        public Housing SelectedHousing { get; set; }
        public List<Notation> NotationsHousing { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnNotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private bool _updatePicture;
        public bool UpdatePicture
        {
            get { return _updatePicture; }
            set
            {
                if (_updatePicture != value)
                {
                    _updatePicture = value;
                }
            }
        }

        private bool _newPictureBox;
        public bool NewPictureBox
        {
            get { return _updatePicture; }
            set
            {
                if (_updatePicture != value)
                {
                    _updatePicture = value;
                }
            }
        }

        private String _newPicture;
        public String NewPicture
        {
            get { return _newPicture; }
            set
            {
                if (_newPicture != value)
                {
                    _newPicture = value;
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
                _localitiesCollection = value;
                RaisePropertyChanged("LocalitiesCollection");
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
        
        [PreferredConstructor]
        public EditHousingViewModel(INavigationService navigationService = null)
        {
            MyFrame = ShellViewModel.MyFrame;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedHousing = (Housing)e.Parameter;
            AssignValue();
        }

        public async Task InitializeContent()
        {
            SelectedHousing = await new HousingService().GetOneHousing(SelectedHousingID);
        }

        public void AssignValue()
        {
            _updatePicture = false;
            _newPictureBox = _updatePicture;
            _newPicture = null;
            SelectedLocality = null;
            _bedTypeCollection = ViewModelLocator.BedsList;
            foreach (var bed in _bedTypeCollection)
            {
                if (SelectedHousing.BedType == bed.Code)
                {
                    _selectedBedType = bed;
                    break;
                }
            }
            _localitiesCollection = ViewModelLocator.LocalitiesList;
            foreach (var loca in _localitiesCollection)
            {
                if (SelectedHousing.City.Equals(loca.Name))
                {
                    _selectedLocality = loca;
                    break;
                }
            }
            Description = SelectedHousing.Description;
            StartDay = new DateTimeOffset(SelectedHousing.StartDate);
            EndDay = new DateTimeOffset(SelectedHousing.EndDate);
            StartTime = new TimeSpan(SelectedHousing.StartDate.Hour, SelectedHousing.StartDate.Minute, SelectedHousing.StartDate.Second);
            EndTime = new TimeSpan(SelectedHousing.EndDate.Hour, SelectedHousing.EndDate.Minute, SelectedHousing.EndDate.Second);
        }

        private ICommand _deleteHousingCommand;
        public ICommand DeleteHousingCommand
        {
            get
            {
                if (this._deleteHousingCommand == null)
                {
                    this._deleteHousingCommand = new RelayCommand(() => DeleteConfirmation());
                }
                return this._deleteHousingCommand;
            }
        }

        private async void DeleteConfirmation()
        {
            if (CanExecute())
            {
                MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir supprimer ce logement ?", "Confirmation de suppression");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = DeleteHousing;
                msgDialog.Commands.Add(okBtn);

                UICommand cancelBtn = new UICommand("Non");
                msgDialog.Commands.Add(cancelBtn);

                await msgDialog.ShowAsync();
            }
        }
        private async void DeleteHousing(IUICommand command)
        {
            await new HousingService().DeleteOneHousing(SelectedHousing.ID);
            MyFrame.Navigate(typeof(HomePage));
        }

        private ICommand _saveHousingCommand;

        public ICommand SaveHousingCommand
        {
            get
            {
                if (this._saveHousingCommand == null)
                {
                    this._saveHousingCommand = new RelayCommand(() => SaveConfirmation());
                }
                return this._saveHousingCommand;
            }
        }

        private async void SaveConfirmation()
        {
            try
            {
                EditedHousing = new HousingPost();
                int numPB = 0;

                EditedHousing.Kitchen = SelectedHousing.Kitchen;
                EditedHousing.Wifi = SelectedHousing.Wifi;
                EditedHousing.Office = SelectedHousing.Office;
                EditedHousing.Shower = SelectedHousing.Shower;
                EditedHousing.Toilet = SelectedHousing.Toilet;

                EditedHousing.StartDate = new DateTime(StartDay.Year, StartDay.Month, StartDay.Day, StartTime.Hours, StartTime.Minutes, 0);
                EditedHousing.EndDate = new DateTime(EndDay.Year, EndDay.Month, EndDay.Day, EndTime.Hours, EndTime.Minutes, 0);

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
                EditedHousing.BedType = SelectedBedType.Code;
                if (!String.IsNullOrWhiteSpace(Description))
                {
                    EditedHousing.Description = Description;
                }
                else
                {
                    EditedHousing.Description = null;
                }
                if (String.IsNullOrWhiteSpace(SelectedHousing.Number))
                {
                    throw new AddressException(1);
                }
                EditedHousing.Number = SelectedHousing.Number;
                if (!String.IsNullOrWhiteSpace(strPostBox))
                {
                    bool parsed = Int32.TryParse(strPostBox, out numPB);
                    if (!parsed)
                    {
                        throw new AddressException(2);
                    }
                }
                if (String.IsNullOrWhiteSpace(SelectedHousing.Street))
                {
                    throw new AddressException(5);
                }
                else
                {
                    EditedHousing.Street = SelectedHousing.Street;
                }
                if (SelectedLocality != null)
                {
                    EditedHousing.ZipCode = SelectedLocality.Zip;
                    EditedHousing.City = SelectedLocality.Name;
                }
                else
                {
                    throw new AddressException(9);
                }
                if (!String.IsNullOrWhiteSpace(NewPicture))
                {
                    EditedHousing.Pictures = NewPicture;
                }
                else
                {
                    EditedHousing.Pictures = SelectedHousing.Pictures;
                }
                EditedHousing.PostBox = numPB;

                MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir sauvegarder les modifications effectuées sur ce logement ?", "Confirmation de modification");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = SaveHousing;
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

        private async void SaveHousing(IUICommand command)
        {
            await new HousingService().EditHousing(SelectedHousing.ID, EditedHousing);
            MyFrame.GoBack();
        }

        public bool CanExecute()
        {
            return (SelectedHousing == null) ? false : true;
        }

        private ICommand _backToHousingsCommand;
        public ICommand BackToHousingsCommand
        {
            get
            {
                if (_backToHousingsCommand == null)
                {
                    _backToHousingsCommand = new RelayCommand(() => BackToHousings());
                }
                return _backToHousingsCommand;
            }
        }

        public void BackToHousings()
        {
            MyFrame.GoBack();
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
