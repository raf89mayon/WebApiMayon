using HomeSnailHome.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class UserPage : Page
    {
        public UserPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((UserViewModel)DataContext).OnNavigatedTo(e);
        }
    }
}
