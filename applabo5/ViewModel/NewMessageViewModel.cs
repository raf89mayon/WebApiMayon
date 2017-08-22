using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.ViewModel
{
    public class NewMessageViewModel : ViewModelBase
    {
        private DispatcherTimer _dispatcherTimer;
        public List<Message> _discussionMessageList;
        public List<Message> DiscussionMessageList
        {
            get { return _discussionMessageList; }
            set
            {
                if (_discussionMessageList != value)
                {
                    _discussionMessageList = value;
                    RaisePropertyChanged("DiscussionMessageList");
                }
            }
        }

        public List<Message> RefreshDiscussionMessageList;

        public Conversation SelectedConversation { get; set; }

        private String _messageContent;
        public String MessageContent
        {
            get { return _messageContent; }
            set
            {
                if (_messageContent != value)
                {
                    _messageContent = value;
                    RaisePropertyChanged("MessageContent");
                }
            }
        }

        [PreferredConstructor]
        public NewMessageViewModel(INavigationService navigationService = null)
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            _dispatcherTimer.Tick += UpdateMessages;
            _dispatcherTimer.Start();
        }

        private async void UpdateMessages(object sender, object e)
        {
            RefreshDiscussionMessageList = new List<Message>(await new MessageService().GetOneConversationMessages(SelectedConversation.Correspondant.ID, SelectedConversation.Subject.ID));
            if (RefreshDiscussionMessageList.Count != DiscussionMessageList.Count)
                DiscussionMessageList = RefreshDiscussionMessageList;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedConversation = (Conversation)e.Parameter;
            DiscussionMessageList = SelectedConversation.Messages;
        }

        public void OnNavigatedFrom()
        {
            _dispatcherTimer.Stop();
        }

        private ICommand _sendMessageCommand;

        public ICommand SendMessageCommand
        {
            get
            {
                if (this._sendMessageCommand == null)
                {
                    this._sendMessageCommand = new RelayCommand(() => SendMessage());
                }
                return this._sendMessageCommand;
            }
        }

        private ICommand _backToConversationCommand;

        public ICommand BackToConversationCommand
        {
            get
            {
                if (this._backToConversationCommand == null)
                {
                    this._backToConversationCommand = new RelayCommand(() => BackToConversation());
                }
                return this._backToConversationCommand;
            }
        }

        public void BackToConversation()
        {
            ShellViewModel.MyFrame.GoBack();
        }

        private async void SendMessage()
        {
            ProgressBarActivity(true);
            if (String.IsNullOrWhiteSpace(MessageContent))
            {
                await new MessageDialog("Veuillez taper un message d'abord", "Envoi impossible").ShowAsync();
            }
            else
            {
                if (CanExecute())
                {
                    await new MessageService().SendMessage(ViewModelLocator.ConnectedUser.ID, SelectedConversation.Correspondant.ID, SelectedConversation.Subject.ID, MessageContent);
                    MessageContent = null;
                    DiscussionMessageList = new List<Message>(await new MessageService().GetOneConversationMessages(SelectedConversation.Correspondant.ID, SelectedConversation.Subject.ID));
                }
            }
            ProgressBarActivity(false);
        }

        public bool CanExecute()
        {
            return (SelectedConversation == null) ? false : true;
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
