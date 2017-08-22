using HomeSnailHome.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using HomeSnailHome.Views;
using HomeSnailHome.Exceptions;

namespace HomeSnailHome.ViewModel
{
    public class HousingSearchViewModel : ViewModelBase
    {
        public Frame MyFrame;

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

        private Bed _selectedBedType;
        public Bed SelectedBedType
        {
            get {
                return _selectedBedType;
            }
            set
            {
                _selectedBedType = value;
                RaisePropertyChanged("SelectedBedType");
            }
        }

        private SearchInfo _newSearch;
        public SearchInfo NewSearch
        {
            get
            {
                return _newSearch;
            }
            set
            {
                if (value != _newSearch)
                {
                    _newSearch = value;
                    OnNotifyPropertyChanged("NewSearch");
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

        private DateTime _today = DateTime.Now;

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
        
        public HousingSearchViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
            NewSearch = new SearchInfo
            {
                Wifi = false,
                Kitchen = false,
                Office = false,
                Shower = false,
                Toilet = false,
                
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,

                BedType = 1
            };
            StartDay = new DateTimeOffset(_today);
            EndDay = new DateTimeOffset(_today);
            StartTime = new TimeSpan(14,00,00);
            EndTime = new TimeSpan(14,00,00);
            _wifiChecked = false;
            _officeChecked = false;
            _showerChecked = false;
            _toiletChecked = false;
            _kitchenChecked = false;
            _bedTypeCollection = ViewModelLocator.BedsList;
            _selectedBedType = _bedTypeCollection[0];
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

        private void CancelConfirmation()
        {
            MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir annuler votre recherche ?", "Annulation");

            UICommand okBtn = new UICommand("Oui");
            okBtn.Invoked = GoBackToHome;
            msgDialog.Commands.Add(okBtn);

            UICommand cancelBtn = new UICommand("Non");
            msgDialog.Commands.Add(cancelBtn);

            msgDialog.ShowAsync();
        }

        private void GoBackToHome(IUICommand command)
        {
            MyFrame.GoBack();
        }

        private ICommand _goNextCommand;
        public ICommand GoNextCommand
        {
            get
            {
                if (_goNextCommand == null)
                {
                    _goNextCommand = new RelayCommand(() => SearchConfirmation());
                }

                return _goNextCommand;
            }
        }

        private async void SearchConfirmation()
        {
            try
            {
                NewSearch.StartDate = new DateTime(StartDay.Year, StartDay.Month, StartDay.Day, StartTime.Hours, StartTime.Minutes, 0);
                NewSearch.EndDate = new DateTime(EndDay.Year, EndDay.Month, EndDay.Day, EndTime.Hours, EndTime.Minutes, 0);
                NewSearch.BedType = SelectedBedType.Code;

                if (NewSearch.StartDate > NewSearch.EndDate)
                {
                    throw new SearchHousingException(1);
                }
                if (NewSearch.StartDate == NewSearch.EndDate)
                {
                    throw new SearchHousingException(2);
                }
                if (NewSearch.BedType == 0)
                {
                    throw new SearchHousingException(3);
                }

                NewSearch.Kitchen = KitchenChecked;
                NewSearch.Wifi = WifiChecked;
                NewSearch.Office = OfficeChecked;
                NewSearch.Shower = ShowerChecked;
                NewSearch.Toilet = ToiletChecked;

                MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir effectuer une recherche ?", "Confirmation");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = GoNext;
                msgDialog.Commands.Add(okBtn);

                UICommand cancelBtn = new UICommand("Non");
                msgDialog.Commands.Add(cancelBtn);

                await msgDialog.ShowAsync();
            }
            catch (SearchHousingException searchEx)
            {
                await new MessageDialog(searchEx.ErrorMessage, "Erreur").ShowAsync();
            }
        }

        private List<Housing> _searchResults;

        public List<Housing> SearchResults
        {
            get
            {
                return _searchResults;
            }
            set
            {
                _searchResults = value;
                if (_searchResults != null)
                {
                    RaisePropertyChanged("SearchResults");
                }
            }
        }

        private async void GoNext(IUICommand command)
        {
            ProgressBarActivity(true);
            SearchResults = new List<Housing>(await new HousingService().GetSearchResults(NewSearch));
            String resultText;

            if (SearchResults.Count > 0)
            {
                if (SearchResults.Count == 1)
                {
                    resultText = "Un seul logement répond à vos critères, voulez-vous afficher ses détails ?";
                }
                else
                {
                    resultText = SearchResults.Count + " logements répondent à vos critères, voulez-vous afficher la liste";                    
                }
                MessageDialog msgDialog = new MessageDialog(resultText, "Resultat");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = DisplayHousing;
                msgDialog.Commands.Add(okBtn);

                UICommand cancelBtn = new UICommand("Non");
                msgDialog.Commands.Add(cancelBtn);

                await msgDialog.ShowAsync();
            }
            else
                await new MessageDialog("Aucun logement répond à vos critères", "Resultat").ShowAsync();
            ProgressBarActivity(false);
        }

        private async void DisplayHousing(IUICommand command)
        {
            if (SearchResults.Count == 1)
            {
                List<Object> obj = new List<Object>()
                {
                    SearchResults[0],
                    new List<Notation>(await new NotationService().GetAllNotationsOneHousing(SearchResults[0].ID))
                };
                MyFrame.Navigate(typeof(DetailHousingPage), obj);
            }
            else
                MyFrame.Navigate(typeof(HousingResultsPage), SearchResults);
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
