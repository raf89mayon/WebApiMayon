using HomeSnailHome.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class DetailHousingPage : Page
    {
        public DetailHousingPage()
        {
            InitializeComponent();
            DetailHousingViewModel.GridManager(this.GridWindows);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((DetailHousingViewModel)DataContext).OnNavigatedTo(e);
        }
    }
}
