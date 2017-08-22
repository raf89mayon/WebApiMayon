using HomeSnailHome.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Service;
using Windows.UI.Xaml.Controls;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class HousingResultsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public Frame MyFrame;
        private Boolean AddDateOrderChro, AvailableDateOrderChro;
        private List<Housing> _housingResults;

        public List<Housing> HousingResults
        {
            get
            {
                return _housingResults;
            }
            set
            {
                _housingResults = value;
                RaisePropertyChanged("HousingResults");
            }
        }
        
        [PreferredConstructor]
        public HousingResultsViewModel(INavigationService navigationService = null)
        {
            MyFrame = ShellViewModel.MyFrame;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            AddDateOrderChro = true;
            AvailableDateOrderChro = false;
            HousingResults = (List<Housing>)e.Parameter;
        }

        private Housing _selectedHousing;

        public Housing SelectedHousing
        {
            get
            {
                return _selectedHousing;
            }
            set
            {
                _selectedHousing = value;
                RaisePropertyChanged("SelectedHousing");
            }
        }

        private ICommand _detailHousingCommand;

        public ICommand DetailHousingCommand
        {
            get
            {
                if (this._detailHousingCommand == null)
                {
                    this._detailHousingCommand = new RelayCommand(() => DisplayHousing());
                }
                return this._detailHousingCommand;
            }
        }

        private async void DisplayHousing()
        {
            if (CanExecute())
            {
                List<Object> obj = new List<Object>()
                {
                    SelectedHousing,
                    new List<Notation>(await new NotationService().GetAllNotationsOneHousing(SelectedHousing.ID))
                };
                MyFrame.Navigate(typeof(DetailHousingPage), obj);
            }
        }

        public bool CanExecute()
        {
            return (SelectedHousing == null) ? false : true;
        }

        private ICommand _goBackCommand;

        public ICommand GoBackCommand
        {
            get
            {
                if (this._goBackCommand == null)
                {
                    this._goBackCommand = new RelayCommand(() => GoBack());
                }
                return this._goBackCommand;
            }
        }

        private void GoBack()
        {
            MyFrame.GoBack();
        }

        private ICommand _orderAvailableDateCommand;

        public ICommand OrderAvailableDateCommand
        {
            get
            {
                if (this._orderAvailableDateCommand == null)
                {
                    this._orderAvailableDateCommand = new RelayCommand(() => OrderHousingsByAvailableDate());
                }
                return this._orderAvailableDateCommand;
            }
        }

        private void OrderHousingsByAvailableDate()
        {
            HousingResults = HousingResults.OrderBy(h => h.StartDate).ToList();
            if (AvailableDateOrderChro)
            {
                HousingResults.Reverse();
                AvailableDateOrderChro = false;
            }
            else
            {
                AvailableDateOrderChro = true;
            }
            AddDateOrderChro = false;
        }

        private ICommand _orderAddDateCommand;

        public ICommand OrderAddDateCommand
        {
            get
            {
                if (this._orderAddDateCommand == null)
                {
                    this._orderAddDateCommand = new RelayCommand(() => OrderHousingsByAddDate());
                }
                return this._orderAddDateCommand;
            }
        }

        private void OrderHousingsByAddDate()
        {
            HousingResults = HousingResults.OrderBy(h => h.AddDate).ToList();
            if (AddDateOrderChro)
            {
                HousingResults.Reverse();
                AddDateOrderChro = false;
            }
            else
            {
                AddDateOrderChro = true;
            }
            AvailableDateOrderChro = false;
        }
    }
}
