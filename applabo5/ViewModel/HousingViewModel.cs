using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class HousingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Boolean AddDateOrderChro, AvailableDateOrderChro;
        public Frame MyFrame;

        private List<Housing> _housingsList;
        public List<Housing> HousingsList
        {
            get
            {
                return _housingsList;
            }
            set
            {
                _housingsList = value;
                RaisePropertyChanged("HousingsList");
            }
        }
        
        public HousingViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
            AddDateOrderChro = false;
            AvailableDateOrderChro = false;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            HousingsList = new List<Housing>(await new HousingService().GetAllHousings());
        }

        private Housing _selectedHousing;

        public Housing SelectedHousing
        {
            get {
                return _selectedHousing;
            }
            set
            {
                _selectedHousing = value;
                if (_selectedHousing != null)
                {
                    RaisePropertyChanged("SelectedHousing");
                }
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
            HousingsList = HousingsList.OrderBy(h => h.StartDate).ToList();
            if (AvailableDateOrderChro)
            {
                HousingsList.Reverse();
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
            HousingsList = HousingsList.OrderBy(h => h.AddDate).ToList();
            if (AddDateOrderChro)
            {
                HousingsList.Reverse();
                AddDateOrderChro = false;
            }
            else
            {
                AddDateOrderChro = true;
            }
            AvailableDateOrderChro = false;
        }

        private async void DisplayHousing()
        {
            if (CanExecute())
            {
                SelectedHousing = await new HousingService().GetOneHousing(SelectedHousing.ID);
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
    }
}
