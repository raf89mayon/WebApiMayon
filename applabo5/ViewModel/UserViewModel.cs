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
    public class UserViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private Boolean AddDateOrderChro, AvailableDateOrderChro;
        public Frame MyFrame;

        private List<User> _userList;
        public List<User> UserList
        {
            get
            {
                return _userList;
            }
            set
            {
                _userList = value;
                RaisePropertyChanged("UserList");
            }
        }

        public UserViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
            AddDateOrderChro = false;
            AvailableDateOrderChro = false;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            UserList = new List<User>(await new UserService().GetAllUsers());
        }

        private User _selectedUser;

        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                if (_selectedUser != null)
                {
                    RaisePropertyChanged("SelectedUser");
                }
            }
        }

        private ICommand _orderAvailableDateCommand;

        public ICommand OrderAvailableDateCommand
        {
            get
            {
                if (this._orderAvailableDateCommand == null)
                {
                    this._orderAvailableDateCommand = new RelayCommand(() => OrderUserByAvailableDate());
                }
                return this._orderAvailableDateCommand;
            }
        }

        private void OrderUserByAvailableDate()
        {
            UserList = UserList.OrderBy(h => h.RegistrationDate).ToList();
            if (AvailableDateOrderChro)
            {
                UserList.Reverse();
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
                    this._orderAddDateCommand = new RelayCommand(() => OrderUserByAddDate());
                }
                return this._orderAddDateCommand;
            }
        }

        private void OrderUserByAddDate()
        {
            UserList = UserList.OrderBy(h => h.LastSignInDate).ToList();
            if (AddDateOrderChro)
            {
                UserList.Reverse();
                AddDateOrderChro = false;
            }
            else
            {
                AddDateOrderChro = true;
            }
            AvailableDateOrderChro = false;
        }

        private ICommand _contactUserCommand;

        public ICommand ContactUserCommand
        {
            get
            {
                if (this._contactUserCommand == null)
                {
                    this._contactUserCommand = new RelayCommand(() => ContactUser());
                }
                return this._contactUserCommand;
            }
        }

        private async void ContactUser()
        {
            if (ViewModelLocator.ConnectedUser.ID == SelectedUser.ID)
            {
                await new MessageDialog("Vous ne pouvez pas vous envoyer des messages", "Accès interdit").ShowAsync();
                return;
            }
            if (CanExecute())
            {
                Conversation Contact = new Conversation()
                {
                    Correspondant = SelectedUser,
                    Subject = new Housing { ID = 0 },
                    Messages = new List<Message>(await new MessageService().GetOneConversationMessages(SelectedUser.ID, 0))
                };
                MyFrame.Navigate(typeof(NewMessagePage), Contact);
            }
        }

        public bool CanExecute()
        {
            return (SelectedUser == null) ? false : true;
        }
    }
}