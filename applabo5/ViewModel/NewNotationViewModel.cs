using HomeSnailHome.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight;
using Windows.UI.Popups;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class NewNotationViewModel : ViewModelBase
    {
        public NotationPost NotationToPost { get; set; }

        public Housing SelectedHousing { get; set; }

        private String _commentContent;
        public String CommentContent
        {
            get { return _commentContent; }
            set
            {
                if (_commentContent != value)
                {
                    _commentContent = value;
                    RaisePropertyChanged("CommentContent");
                }
            }
        }

        private float _quotationValue;
        public float QuotationValue
        {
            get { return _quotationValue; }
            set
            {
                if (_quotationValue != value)
                {
                    _quotationValue = value;
                    RaisePropertyChanged("QuotationValue");
                }
            }
        }
        
        [PreferredConstructor]
        public NewNotationViewModel(INavigationService navigationService = null)
        {
            QuotationValue = 5;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectedHousing = (Housing)e.Parameter;
        }

        private ICommand _sendNotationCommand;
        public ICommand SendNotationCommand
        {
            get
            {
                if (_sendNotationCommand == null)
                {
                    _sendNotationCommand = new RelayCommand(() => SendNotation());
                }
                return _sendNotationCommand;
            }
        }

        public Boolean ValidateNotation()
        {
            if (!String.IsNullOrWhiteSpace(CommentContent))
            {
                NotationToPost = new NotationPost()
                {
                    OriginID = ViewModelLocator.ConnectedUser.ID,
                    HousingID = SelectedHousing.ID,
                    Comment = CommentContent,
                    Quotation = QuotationValue
                };
                return true;
            }
            else
            {
                new MessageDialog("Veuillez justifier votre cotation via un commentaire", "Erreur").ShowAsync();
                return false;
            }
        }

        private ICommand _backToHousingCommand;

        public ICommand BackToHousingCommand
        {
            get
            {
                if (_backToHousingCommand == null)
                {
                    _backToHousingCommand = new RelayCommand(() => BackToHousingDetail());
                }
                return _backToHousingCommand;
            }
        }

        public async void BackToHousingDetail()
        {
            ShellViewModel.MyFrame.Navigate(typeof(DetailHousingPage), new List<Object>() { SelectedHousing, new List<Notation>(await new NotationService().GetAllNotationsOneHousing(SelectedHousing.ID)) });
        }

        private async void SendNotation()
        {
            ProgressBarActivity(true);
            if (ValidateNotation() == true)
            {
                await new NotationService().SendNotation(NotationToPost);
                CommentContent = "";
                QuotationValue = 5;
                BackToHousingDetail();
            }
            ProgressBarActivity(false);
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
