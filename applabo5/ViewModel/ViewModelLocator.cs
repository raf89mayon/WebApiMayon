using HomeSnailHome.Model;
using HomeSnailHome.Service;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HomeSnailHome.Views;

namespace HomeSnailHome.ViewModel
{
    public class ViewModelLocator
    {
        public static User ConnectedUser;
        public static ObservableCollection<Locality> LocalitiesList;
        public static ObservableCollection<Bed> BedsList;

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<HousingViewModel>();
            SimpleIoc.Default.Register<MyHousingViewModel>();
            SimpleIoc.Default.Register<DetailHousingViewModel>();
            SimpleIoc.Default.Register<NewNotationViewModel>();
            SimpleIoc.Default.Register<NewUserViewModel>();
            SimpleIoc.Default.Register<NewHousingViewModel>();
            SimpleIoc.Default.Register<EditHousingViewModel>();
            SimpleIoc.Default.Register<HousingSearchViewModel>();
            SimpleIoc.Default.Register<HousingResultsViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<ConversationListViewModel>();
            SimpleIoc.Default.Register<NewMessageViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();

            NavigationService navigationPages = new NavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => navigationPages);
            navigationPages.Configure("MainPage", typeof(MainPage));
            navigationPages.Configure("HomePage", typeof(HomePage));
            navigationPages.Configure("DetailHousingPage", typeof(DetailHousingPage));
            navigationPages.Configure("NewUserPage", typeof(NewUserPage));
            navigationPages.Configure("NewHousingPage", typeof(NewHousingPage));
            navigationPages.Configure("EditHousingPage", typeof(EditHousingPage));
            navigationPages.Configure("HousingPage", typeof(HousingPage));
            navigationPages.Configure("MyHousingPage", typeof(MyHousingPage));
            navigationPages.Configure("HousingSearchPage", typeof(HousingSearchPage));
            navigationPages.Configure("HousingResultsPage", typeof(HousingResultsPage));
            navigationPages.Configure("ConversationListPage", typeof(ConversationListPage));
            navigationPages.Configure("NewNotationPage", typeof(NewNotationPage));
            navigationPages.Configure("NewMessagePage", typeof(NewMessagePage));
            navigationPages.Configure("Shell", typeof(Shell));
        }

        public static async Task InitializeLists()
        {
            LocalitiesList = new ObservableCollection<Locality>(await new LocalityService().GetAllLocalities());
            BedsList = new ObservableCollection<Bed>(await new BedService().GetAllBedTypes());
        }

        public static void SignIn (User userConnected)
        {
            ConnectedUser = userConnected;
        }

        public static void SignOut()
        {
            ConnectedUser = null;
        }

        public static bool IsConnected()
        {
            return (ConnectedUser != null);
        }

        public MainViewModel MainPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public UserViewModel UserPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserViewModel>();
            }
        }

        public HomeViewModel HomePage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomeViewModel>();
            }
        }

        public ShellViewModel Shell
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ShellViewModel>();
            }
        }

        public HousingViewModel HousingPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HousingViewModel>();
            }
        }

        public MyHousingViewModel MyHousingPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyHousingViewModel>();
            }
        }

        public DetailHousingViewModel DetailHousingPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DetailHousingViewModel>();
            }
        }

        public NewUserViewModel NewUserPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewUserViewModel>();
            }
        }

        public NewHousingViewModel NewHousingPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewHousingViewModel>();
            }
        }

        public EditHousingViewModel EditHousingPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditHousingViewModel>();
            }
        }

        public HousingSearchViewModel HousingSearchPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HousingSearchViewModel>();
            }
        }

        public HousingResultsViewModel HousingResultsPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HousingResultsViewModel>();
            }
        }

        public ConversationListViewModel ConversationListPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ConversationListViewModel>();
            }
        }

        public NewNotationViewModel NewNotationPage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewNotationViewModel>();
            }
        }

        public NewMessageViewModel NewMessagePage
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NewMessageViewModel>();
            }
        }
    }
}
