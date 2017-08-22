using HomeSnailHome.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using HomeSnailHome.Exceptions;
using Windows.UI.Popups;
using HomeSnailHome.Views;
using System.Text.RegularExpressions;

namespace HomeSnailHome.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private String _email;
        public String Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }        

        private String _password;
        public String Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }
        
        public MainViewModel(INavigationService navigationService)
        {
            Email = "";
            Password = "";
            ProgressBarActivity(false);
        }

        private ICommand _connectionCommand;
        public ICommand ConnectionCommand
        {
            get
            {
                if (_connectionCommand == null)
                {
                    _connectionCommand = new RelayCommand(async () => await Connection());
                }
                return _connectionCommand;
            }
        }

        public async Task Connection()
        {
            ProgressBarActivity(true);

            var loginService = new LoginService();
            try
            {
                var token = await loginService.GetToken(Email,Password);
                if (token != null)
                {
                    User userLogIn = await new UserService().GetOneUser(Email);
                    if (userLogIn != null)
                    {
                        ViewModelLocator.SignIn(userLogIn);
                        await ViewModelLocator.InitializeLists();
                        await new MessageDialog("Bienvenue " + userLogIn.FirstName + " " + userLogIn.LastName, "Connexion réussie").ShowAsync();
                        Email = "";
                        Password = "";
                        ShellViewModel.MyFrame.Navigate(typeof(HomePage));
                    }
                    else
                    {
                        await new MessageDialog("Identifiant et/ou mot de passe incorrect(s)", "Erreur").ShowAsync();
                    }
                }
            }
            catch (ApiRequestFailedException)
            {
                await new MessageDialog("Impossible d'accéder à la base de données", "Erreur").ShowAsync();
            }
            ProgressBarActivity(false);
        }
        
        private ICommand _createNewUserCommand;
        public ICommand CreateNewUserCommand
        {
            get
            {
                if (_createNewUserCommand == null)
                {
                    _createNewUserCommand = new RelayCommand(() => CreateNewUser());
                }
                return _createNewUserCommand;
            }
        }

        private void CreateNewUser()
        {
            ShellViewModel.MyFrame.Navigate(typeof(NewUserPage));
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
