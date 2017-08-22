using HomeSnailHome.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Windows.Input;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using System.ComponentModel;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using HomeSnailHome.Exceptions;
using HomeSnailHome.Views;
using AppLabo5.Exceptions;
using System.Text.RegularExpressions;
using System.Globalization;

namespace HomeSnailHome.ViewModel
{
    public class NewUserViewModel : ViewModelBase
    {
        public Frame MyFrame;

        private UserPost _newUser;
        public UserPost NewUser
        {
            get
            {
                return _newUser;
            }
            set
            {
                if (value != _newUser)
                {
                    _newUser = value;
                    OnNotifyPropertyChanged("NewUser");
                }
            }
        }

        private String _passWord;
        public String PassWord
        {
            get
            {
                return _passWord;
            }
            set
            {
                _passWord = value;
                OnNotifyPropertyChanged("PassWord");
            }
        }

        private String _confirmPassWord;
        public String ConfirmPassWord
        {
            get
            {
                return _confirmPassWord;
            }
            set
            {
                _confirmPassWord = value;
                OnNotifyPropertyChanged("ConfirmPassWord");
            }
        }

        private DateTime _today = DateTime.Now;
        
        private DateTimeOffset _birthdate;
        public DateTimeOffset Birthdate
        {
            get
            {
                return _birthdate;
            }
            set
            {                
                _birthdate = value;
                OnNotifyPropertyChanged("Birthdate");
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

        public Boolean UserIDExisting { get; set; }
        
        public NewUserViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
            UserIDExisting = false;
            NewUser = new UserPost
            {
                ID = null,
/**/            PassWord = null,
                LastName = null,
                FirstName = null,
                BirthDate = _today,
 /**/           EmailAddress = null,
                PhoneNumber = null,
                Number = null,
                PostBox = 0,
                Street = null,
                ZipCode = 0,
                City = null,
                Country = null,
                RoleID = 3,
                Picture = null
            };
            _confirmPassWord = null;
            Birthdate = new DateTimeOffset(_today.AddYears(-15));
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
            okBtn.Invoked = GoBackToHome;
            msgDialog.Commands.Add(okBtn);

            UICommand cancelBtn = new UICommand("Non");
            msgDialog.Commands.Add(cancelBtn);

            await msgDialog.ShowAsync();
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
                    _goNextCommand = new RelayCommand(() => TokenConnection());
                }

                return _goNextCommand;
            }
        }

        public async Task TokenConnection()
        {
            var loginService = new LoginService();

            try
            {
                var token = await loginService.GetToken("admin@homesnailhome.be", "Smart_2016");
                if (token != null)
                {
                    await AddConfirmation();
                }
            }
            catch (ApiRequestFailedException)
            {
            }
        }

        private async Task AddConfirmation()
        {
            ProgressBarActivity(true);
            if (!String.IsNullOrWhiteSpace(NewUser.ID)) await ExistingUserID(NewUser.ID);
            
            try
            {
                int numPB = 0, numCP = 0;
                if (String.IsNullOrWhiteSpace(NewUser.ID))
                {
                    throw new AddUserException(1);
                }
                if (!IsValidEmail(NewUser.ID))
                {
                    throw new AddUserException(2);
                }
                if (UserIDExisting == true)
                {
                    throw new AddUserException(3);
                }
                if (String.IsNullOrWhiteSpace(PassWord))
                {
                    throw new AddUserException(4);
                }
                if (String.IsNullOrWhiteSpace(ConfirmPassWord))
                {
                    throw new AddUserException(5);
                }
                if (PassWord.Length < 8)
                {
                    throw new AddUserException(6);
                }
                if (!IsValidPassword(PassWord))
                {
                    throw new AddUserException(7);
                }
                if (!PassWord.Equals(ConfirmPassWord))
                {
                    throw new AddUserException(8);
                }
                if (String.IsNullOrWhiteSpace(NewUser.LastName))
                {
                    throw new AddUserException(9);
                }
                if (String.IsNullOrWhiteSpace(NewUser.FirstName))
                {
                    throw new AddUserException(10);
                }
                if (Birthdate > _today.AddYears(-15))
                {
                    throw new AddUserException(11);
                }
                NewUser.BirthDate = Birthdate.DateTime;
                if (String.IsNullOrWhiteSpace(NewUser.Number))
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
                if (String.IsNullOrWhiteSpace(strZipCode))
                {
                    throw new AddressException(3);
                }
                else
                {
                    bool parsed = Int32.TryParse(strZipCode, out numCP);
                    if (!parsed)
                    {
                        throw new AddressException(4);
                    }
                }
                if (String.IsNullOrWhiteSpace(NewUser.Street))
                {
                    throw new AddressException(5);
                }
                if (String.IsNullOrWhiteSpace(NewUser.City))
                {
                    throw new AddressException(6);
                }
                if (String.IsNullOrWhiteSpace(NewUser.Country))
                {
                    throw new AddressException(7);
                }
                if ((numCP < 1000 || numCP >= 10000) && NewUser.Country.Equals("Belgique"))
                {
                    throw new AddressException(8);
                }
                if (String.IsNullOrWhiteSpace(NewUser.Picture))
                {
                    NewUser.Picture = null;
                }
                /**/
                NewUser.PassWord = PassWord;
                /**/
                NewUser.EmailAddress = NewUser.ID;
                NewUser.PostBox = numPB;
                NewUser.ZipCode = numCP;

                MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir poursuivre la création d'un compte utilisateur ?", "Confirmation");

                UICommand okBtn = new UICommand("Oui");
                okBtn.Invoked = GoNext;
                msgDialog.Commands.Add(okBtn);

                UICommand cancelBtn = new UICommand("Non");
                msgDialog.Commands.Add(cancelBtn);

                await msgDialog.ShowAsync();
            }
            catch (AddUserException addUserEx)
            {
                await DisplayException(addUserEx.ErrorMessage);
            }
            catch (AddressException addressEx)
            {
                await DisplayException(addressEx.ErrorMessage);
            }
            ProgressBarActivity(false);
        }

        private async void GoNext(IUICommand command)
        {
            await new UserService().AddOneUser(NewUser, PassWord);
            MyFrame.Navigate(typeof(MainPage));
        }

        public bool IsValidPassword(string pw)
        {
            if (Regex.IsMatch(pw, @"^[a-zA-Z0-9%_#@]+$"))
            {
                if (Regex.IsMatch(pw, @"^[a-z]+$") ||
                    Regex.IsMatch(pw, @"^[A-Z]+$") ||
                    Regex.IsMatch(pw, @"^[0-9]+$") ||
                    Regex.IsMatch(pw, @"^[%_#@]+$"))
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;
            
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;
            
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        private async Task DisplayException (string message)
        {
            await new MessageDialog(message, "Erreur").ShowAsync();
        }

        public async Task ExistingUserID(String userID)
        {
            var users = await new UserService().GetAllUsers();
            UserIDExisting = false;
            if (users != null)
                foreach (var use in users)
                    if (use.ID.Equals(userID))
                    {
                        UserIDExisting = true;
                        break;
                    }
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
        private bool invalid;

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