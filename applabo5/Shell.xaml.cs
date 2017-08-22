using HomeSnailHome.ViewModel;
using HomeSnailHome.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace HomeSnailHome
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            this.InitializeComponent();
            ShellViewModel.FrameManager(this.MainFrame);
        }

        internal void ShowDefaultPage()
        {
            MainFrame.Navigate(typeof(MainPage));
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        internal bool DoesShowSomething()
        {
            return MainFrame.Content != null;
        }

        private void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(HousingPage));
        }

        private void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(UserPage));
        }

        private void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(MainPage));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
    }
}
