using HomeSnailHome.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class HousingPage : Page
    {
        public HousingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((HousingViewModel)DataContext).OnNavigatedTo(e);
        }
    }
}
