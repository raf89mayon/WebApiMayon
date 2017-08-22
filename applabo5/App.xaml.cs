using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace HomeSnailHome
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Shell shell = Window.Current.Content as Shell;
            if (shell == null)
            {
                shell = new Shell();
                Window.Current.Content = shell;
            }

            if (e.PrelaunchActivated == false)
            {
                if (!shell.DoesShowSomething())
                {
                    shell.ShowDefaultPage();
                }
                Window.Current.Activate();
            }
        }
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}