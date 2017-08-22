using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class DetailHousingViewModel : ViewModelBase
    {
        public static Grid pageGrid;
        public Frame MyFrame;
        public String NotationsTitle { get; set; }
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
        
        [PreferredConstructor]
        public DetailHousingViewModel(INavigationService navigationService = null)
        {
            MyFrame = ShellViewModel.MyFrame;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            List<Object> obj = (List<Object>)e.Parameter;
            SelectedHousing = (Housing)obj.First();
            NotationsHousing = (List<Notation>)obj.Last();

            if (NotationsHousing.Count() > 0)
                NotationsTitle = "Dernières notations des utilisateurs :";
            else
                NotationsTitle = "Ce logement n'a pas encore été noté";

            /*
            if (SelectedHousing.Localization.Status == "OK")
                await UpdateGrid(SelectedHousing.Localization);
            */

            await UpdateGrid();
        }

        public static void GridManager(Grid myGrid)
        {
            pageGrid = myGrid;
        }

        public async Task UpdateGrid()
        {
            IEnumerable<InfoLocal> info = await new LocalService().GetEditedLocalInfos(SelectedHousing);
            if (info != null)
            {
                InfoLocal infoLoc = info.First();
                MapControl newMap = new MapControl();
                newMap.ZoomLevel = 15;
                newMap.Height = 350;
                newMap.MapServiceToken = "d3ogHQxrDviYWT4rgWNQ~2O1SHY9XpbCwof0N7rR2Yg~Ahe26ctwSO5twYSRpsM_5DGl6vWDkrLp1nCBqyE6oAuVF0Bb-oyDCW9ZmnIV_7rm";
                infoLoc.location_lon -= 180;
                infoLoc.northeast_lon -= 180;
                infoLoc.southwest_lon -= 180;
                BasicGeoposition GeoPos_NorthWest = new BasicGeoposition() { Latitude = infoLoc.northeast_lat + 0.05, Longitude = infoLoc.northeast_lon + 0.1 };
                BasicGeoposition GeoPos_SouthEast = new BasicGeoposition() { Latitude = infoLoc.southwest_lat - 0.05, Longitude = infoLoc.southwest_lon - 0.1 };
                MapIcon Icon_HousingLocation = new MapIcon() { Location = new Geopoint(new BasicGeoposition() { Latitude = infoLoc.location_lat, Longitude = (infoLoc.location_lon - 180) }), NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 0.5), ZIndex = 0 };
                Icon_HousingLocation.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mapicon.png"));
                newMap.MapElements.Add(Icon_HousingLocation);
                
                try
                {
                    Geolocator myGeolocator = new Geolocator();
                    Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                    MapIcon Icon_UserLocation = new MapIcon() { Location = new Geopoint(new BasicGeoposition() { Latitude = myGeoposition.Coordinate.Latitude, Longitude = myGeoposition.Coordinate.Longitude }), NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 0.5), ZIndex = 0 };
                    Icon_UserLocation.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/user_map.png"));
                    newMap.MapElements.Add(Icon_UserLocation);
                }
                catch (Exception ex)
                {
                }

                GeoboundingBox mapBox = new GeoboundingBox(GeoPos_NorthWest, GeoPos_SouthEast);
                newMap.Center = new Geopoint(mapBox.Center);
                pageGrid.Children.Add(newMap);
            }
        }
        /*
        public async Task UpdateGrid(InfoLocal infoLoc)
        {
            MapControl newMap = new MapControl();
            newMap.ZoomLevel = 15;
            newMap.Height = 350;
            newMap.MapServiceToken = "d3ogHQxrDviYWT4rgWNQ~2O1SHY9XpbCwof0N7rR2Yg~Ahe26ctwSO5twYSRpsM_5DGl6vWDkrLp1nCBqyE6oAuVF0Bb-oyDCW9ZmnIV_7rm";
            infoLoc.location_lon -= 180;
            infoLoc.northeast_lon -= 180;
            infoLoc.southwest_lon -= 180;
            BasicGeoposition GeoPos_NorthWest = new BasicGeoposition() { Latitude = infoLoc.northeast_lat + 0.05, Longitude = infoLoc.northeast_lon + 0.1 };
            BasicGeoposition GeoPos_SouthEast = new BasicGeoposition() { Latitude = infoLoc.southwest_lat - 0.05, Longitude = infoLoc.southwest_lon - 0.1 };
            MapIcon Icon_HousingLocation = new MapIcon() { Location = new Geopoint(new BasicGeoposition() { Latitude = infoLoc.location_lat, Longitude = (infoLoc.location_lon - 180) }), NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 0.5), ZIndex = 0 };
            Icon_HousingLocation.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/mapicon.png"));
            newMap.MapElements.Add(Icon_HousingLocation);

            try
            {
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                MapIcon Icon_UserLocation = new MapIcon() { Location = new Geopoint(new BasicGeoposition() { Latitude = myGeoposition.Coordinate.Latitude, Longitude = myGeoposition.Coordinate.Longitude }), NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 0.5), ZIndex = 0 };
                Icon_UserLocation.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/user_map.png"));
                newMap.MapElements.Add(Icon_UserLocation);
            }
            catch (Exception ex)
            {
            }

            GeoboundingBox mapBox = new GeoboundingBox(GeoPos_NorthWest, GeoPos_SouthEast);
            newMap.Center = new Geopoint(mapBox.Center);
            pageGrid.Children.Add(newMap);
        }
        */
        private ICommand _contactHousingCommand;
        public ICommand ContactHousingCommand
        {
            get
            {
                if (this._contactHousingCommand == null)
                {
                    this._contactHousingCommand = new RelayCommand(() => ContactUser());
                }
                return this._contactHousingCommand;
            }
        }

        private async void ContactUser()
        {
            if (ViewModelLocator.ConnectedUser.ID == SelectedHousing.Host.ID)
            {
                await new MessageDialog("Vous ne pouvez pas vous envoyer des messages", "Accès interdit").ShowAsync();
                return;
            }
            if (CanExecute())
            {
                Conversation Contact = new Conversation()
                {
                    Correspondant = SelectedHousing.Host,
                    Subject = SelectedHousing,
                    Messages = new List<Message>(await new MessageService().GetOneConversationMessages(SelectedHousing.Host.ID, SelectedHousing.ID))
                };
                MyFrame.Navigate(typeof(NewMessagePage), Contact);
            }
        }

        private ICommand _commentHousingCommand;

        public ICommand CommentHousingCommand
        {
            get
            {
                if (this._commentHousingCommand == null)
                {
                    this._commentHousingCommand = new RelayCommand(() => HousingNotation());
                }
                return this._commentHousingCommand;
            }
        }

        private async void HousingNotation()
        {
            if (ViewModelLocator.ConnectedUser.ID == SelectedHousing.Host.ID)
            {
                await new MessageDialog("Vous ne pouvez pas noter votre propre logement", "Accès interdit").ShowAsync();
                return;
            }

            if (CanExecute())
            {
                MyFrame.Navigate(typeof(NewNotationPage), SelectedHousing);
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
                if (ViewModelLocator.ConnectedUser.ID != SelectedHousing.Host.ID && ViewModelLocator.ConnectedUser.Role.HousingRight != true)
                {
                    await new MessageDialog("Vous n'avez pas le droit de supprimer ce logement", "Accès interdit").ShowAsync();
                    return;
                }
                else
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

        private async void EditHousing()
        {
            if (CanExecute())
            {
                if (ViewModelLocator.ConnectedUser.ID != SelectedHousing.Host.ID)
                {
                    await new MessageDialog("Vous n'avez pas le droit de modifier ce logement", "Accès interdit").ShowAsync();
                    return;
                }
                else
                {
                    MyFrame.Navigate(typeof(EditHousingPage), SelectedHousing);
                }
            }
        }
    }
}
