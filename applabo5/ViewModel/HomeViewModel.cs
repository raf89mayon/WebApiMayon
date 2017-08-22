using HomeSnailHome.Model;
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
    public class HomeViewModel : ViewModelBase
    {
        public Frame MyFrame;
        public User ConnectedUser { get; set; }
        
        [PreferredConstructor]
        public HomeViewModel(INavigationService navigationService = null)
        {
            MyFrame = ShellViewModel.MyFrame;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            ConnectedUser = ViewModelLocator.ConnectedUser;
        }

        private ICommand _signOutCommand;
        public ICommand SignOutCommand
        {
            get
            {
                if (this._signOutCommand == null)
                {
                    this._signOutCommand = new RelayCommand(() => SignOutConfirmation());
                }
                return this._signOutCommand;
            }
        }

        private async void SignOutConfirmation()
        {
            MessageDialog msgDialog = new MessageDialog("Etes-vous sûr de vouloir vous déconnecter ?", "Déconnexion");

            UICommand okBtn = new UICommand("Oui");
            okBtn.Invoked = SignOut;
            msgDialog.Commands.Add(okBtn);

            UICommand cancelBtn = new UICommand("Non");
            msgDialog.Commands.Add(cancelBtn);

            await msgDialog.ShowAsync();
        }

        private void SignOut(IUICommand command)
        {
            ViewModelLocator.SignOut();
            MyFrame.Navigate(typeof(MainPage));
        }
    }
}
