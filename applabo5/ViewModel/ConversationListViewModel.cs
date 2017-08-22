using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class ConversationListViewModel : ViewModelBase
    {
        public Frame MyFrame;
        public User ConnectedUser { get; set; }

        private List<Conversation> _conversationListing;

        public List<Conversation> ConversationListing
        {
            get
            {
                return _conversationListing;
            }
            set
            {
                _conversationListing = value;
                RaisePropertyChanged("ConversationListing");
            }
        }

        private Conversation _selectedConversation;

        public Conversation SelectedConversation
        {
            get
            {
                return _selectedConversation;
            }
            set
            {
                _selectedConversation = value;
                RaisePropertyChanged("SelectedConversation");
            }
        }
        
        [PreferredConstructor]
        public ConversationListViewModel(INavigationService navigationService)
        {
            MyFrame = ShellViewModel.MyFrame;
        }

        public async void OnNavigatedTo(NavigationEventArgs e)
        {
            ConversationListing = null;
            ConnectedUser = ViewModelLocator.ConnectedUser;
            await GetMessages();
        }

        public async Task GetMessages()
        {
            ConversationListing = new List<Conversation>(await new MessageService().GetAllConversationsUser(ConnectedUser.ID));
        }

        private ICommand _openConversationCommand;

        public ICommand OpenConversationCommand
        {
            get
            {
                if (this._openConversationCommand == null)
                {
                    this._openConversationCommand = new RelayCommand(() => NewMessageConversation());
                }
                return this._openConversationCommand;
            }
        }

        private void NewMessageConversation()
        {
            if (CanExecute())
            {
                MyFrame.Navigate(typeof(NewMessagePage), SelectedConversation);
            }
        }

        public bool CanExecute()
        {
            return (SelectedConversation == null) ? false : true;
        }

        private ICommand _backToCommand;

        public ICommand BackToCommand
        {
            get
            {
                if (this._backToCommand == null)
                {
                    this._backToCommand = new RelayCommand(() => BackTo());
                }
                return this._backToCommand;
            }
        }
        public void BackTo()
        {
            MyFrame.Navigate(typeof(HomePage));
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
