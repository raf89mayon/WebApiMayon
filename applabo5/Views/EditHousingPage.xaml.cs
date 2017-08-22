using HomeSnailHome.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome.Views
{
    public sealed partial class EditHousingPage : Page
    {
        public EditHousingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ((EditHousingViewModel)DataContext).OnNavigatedTo(e);
        }

        private void TextBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
