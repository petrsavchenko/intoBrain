using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IntoTheBrain.DataModel;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace IntoTheBrain
{



    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            InitializeLicense();
            this.Suspending += OnSuspending;
        }

        //Информация о лицензии
        private LicenseInformation licenseInformation;
        

        void InitializeLicense()
        {
            // Initialize the license info for use in the app that is uploaded to the Store.
            // uncomment for release
            //   licenseInformation = CurrentApp.LicenseInformation;

            // Initialize the license info for testing.
            // comment the next line for release
            licenseInformation = CurrentApp.LicenseInformation;

            // Register for the license state change event.
            licenseInformation.LicenseChanged += licenseChangedEventHandler;
        }


        void licenseChangedEventHandler()
        {
            ReloadLicense(); // code is in next steps
            var md = new MessageDialog("Вы приобрели кучу головоломок-задач, получайте удовольствие и наслаждайтесь умственной гимнастикой", "Поздравляю");
            md.ShowAsync();
        }


        async void ReloadLicense()
        {
            if (licenseInformation.IsActive)
            {
                if (licenseInformation.IsTrial)
                {
                    // Show the features that are available during trial only.
                    var taskDataSource = (TaskDataSource)App.Current.Resources["taskDataSource"];
                    if (taskDataSource != null)
                    {
                        await taskDataSource.GetDemoTasks();
                            
                        var md = new MessageDialog("Привет\nУ тебя установлена демо версия. Жаль, но она содержит всего 10 задач. Тогда как в полной версии 1001 задача", "Предупреждение о Демо версии");
                            
                        md.Commands.Add(
                            new UICommand("Да, понял я, отстань"));
                        await md.ShowAsync();
                        //var a = Windows.Storage.ApplicationData.Current.LocalFolder;

                    }
                }
                else
                {
                    // Show the features that are available only with a full license.
                    var taskDataSource = (TaskDataSource)App.Current.Resources["taskDataSource"];
                    if (taskDataSource != null)
                    {
                            await taskDataSource.GetTasks();
                    }
                }
            }
            else
            {
                throw new Exception("Ошибка! Откуда у вас приложение? У вас нет лицензии");
                // A license is inactive only when there's an error.
            }
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                ReloadLicense();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
