using HomeSnailHome.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class HousingResultsPage : Page
    {
        public HousingResultsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((HousingResultsViewModel)DataContext).OnNavigatedTo(e);
        }
    }
}
