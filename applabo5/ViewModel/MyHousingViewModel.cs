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
using Windows.UI.Popups;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class MyHousingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Boolean AddDateOrderChro, EditDateOrderChro;
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

        public MyHousingViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
            AddDateOrderChro = false;
            EditDateOrderChro = false;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            HousingsList = new List<Housing>(await new HousingService().GetAllUserHousings(ViewModelLocator.ConnectedUser.ID));
            if (HousingsList.Count() == 0)
                await new MessageDialog("Votre liste de logements est vide", "Aucun Logement disponible").ShowAsync();
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

        private ICommand _orderEditDateCommand;
        public ICommand OrderEditDateCommand
        {
            get
            {
                if (this._orderEditDateCommand == null)
                {
                    this._orderEditDateCommand = new RelayCommand(() => OrderHousingsByEditDate());
                }
                return this._orderEditDateCommand;
            }
        }

        private void OrderHousingsByEditDate()
        {
            HousingsList = HousingsList.OrderBy(h => h.EditDate).ToList();
            if (EditDateOrderChro)
            {
                HousingsList.Reverse();
                EditDateOrderChro = false;
            }
            else
            {
                EditDateOrderChro = true;
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
            EditDateOrderChro = false;
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

        private ICommand _editHousingCommand;

        public ICommand EditHousingCommand
        {
            get
            {
                if (this._editHousingCommand == null)
                {
                    this._editHousingCommand = new RelayCommand(() => EditHousing());
                }
                return this._editHousingCommand;
            }
        }

        private void EditHousing()
        {
            if (CanExecute())
            {
                MyFrame.Navigate(typeof(EditHousingPage), SelectedHousing);
            }
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

        public bool CanExecute()
        {
            return (SelectedHousing == null) ? false : true;
        }
    }
}
