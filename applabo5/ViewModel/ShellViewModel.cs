using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        [PreferredConstructor]
        public ShellViewModel(INavigationService navigationService = null)
        {
        }
        
        public void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public static Frame MyFrame;
        public static void FrameManager(Frame myFrame)
        {
            MyFrame = myFrame;
        }

        private ICommand _usersCommand;
        public ICommand UsersCommand
        {
            get
            {
                if (_usersCommand == null)
                {
                    _usersCommand = new RelayCommand(() => UserList());
                }
                return _usersCommand;
            }
        }

        public void UserList()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(UserPage));
            else
                ConnectionMessage();
        }

        private ICommand _myHousingCommand;
        public ICommand MyHousingCommand
        {
            get
            {
                if (_myHousingCommand == null)
                {
                    _myHousingCommand = new RelayCommand(() => MyHousingList());
                }
                return _myHousingCommand;
            }
        }

        public void MyHousingList()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(MyHousingPage));
            else
                ConnectionMessage();
        }

        private ICommand _housingListCommand;
        public ICommand HousingListCommand
        {
            get
            {
                if (_housingListCommand == null)
                {
                    _housingListCommand = new RelayCommand(() => HousingList());
                }
                return _housingListCommand;
            }
        }

        public void HousingList()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(HousingPage));
            else
                ConnectionMessage();
        }

        private ICommand _housingSearchCommand;
        public ICommand HousingSearchCommand
        {
            get
            {
                if (_housingSearchCommand == null)
                {
                    _housingSearchCommand = new RelayCommand(() => HousingSearch());
                }
                return _housingSearchCommand;
            }
        }

        public void HousingSearch()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(HousingSearchPage));
            else
                ConnectionMessage();
        }

        private ICommand _housingAddCommand;
        public ICommand HousingAddCommand
        {
            get
            {
                if (_housingAddCommand == null)
                {
                    _housingAddCommand = new RelayCommand(() => HousingAdd());
                }
                return _housingAddCommand;
            }
        }

        public void HousingAdd()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(NewHousingPage));
            else
                ConnectionMessage();
        }

        private ICommand _conversationCommand;
        public ICommand ConversationCommand
        {
            get
            {
                if (_conversationCommand == null)
                {
                    _conversationCommand = new RelayCommand(() => ConversationList());
                }
                return _conversationCommand;
            }
        }

        public void ConversationList()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(ConversationListPage));
            else
                ConnectionMessage();
        }

        private ICommand _homeCommand;
        public ICommand HomeCommand
        {
            get
            {
                if (_homeCommand == null)
                {
                    _homeCommand = new RelayCommand(() => Home());
                }
                return _homeCommand;
            }
        }

        public void Home()
        {
            if (ViewModelLocator.IsConnected())
                MyFrame.Navigate(typeof(HomePage));
            else
                MyFrame.Navigate(typeof(MainPage));
        }

        private ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                if (_aboutCommand == null)
                {
                    _aboutCommand = new RelayCommand(() => About());
                }
                return _aboutCommand;
            }
        }

        public void About()
        {
            MyFrame.Navigate(typeof(About));
        }

        public async void ConnectionMessage()
        {
            await new MessageDialog("Veuillez vous connecter ou créer un compte afin d'avoir accès aux fonctionalités de HomeSnailHome", "Accès interdit").ShowAsync();
        }
    }
}