using HomeSnailHome.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class MyHousingPage : Page
    {
        public MyHousingPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((MyHousingViewModel)DataContext).OnNavigatedTo(e);
        }
    }
}
