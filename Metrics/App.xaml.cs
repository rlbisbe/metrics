using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Metrics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public event EventHandler LoadCompleted;

        private ObservableCollection<Widget> _widgets = new ObservableCollection<Widget>();
        public ObservableCollection<Widget> Widgets
        {
            get
            {
                return this._widgets;
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
       public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            object NumWidgets = localSettings.Values["NumWidgets"];

            if (NumWidgets != null)
            {
                for (int i = 0; i < (int)NumWidgets; i++)
                {
                    ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)localSettings.Values["Widget" + i];
                    Widget w = Widget.CreateWidget(composite);
                    w.Update();
                    this.Widgets.Add(w);
                }
            }

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();

            //myApp.Widgets.Add(new Widget() { Title = "Followers for @rlbisbe", Counter = 200, Background = "Green" });
            //myApp.Widgets.Add(new Widget() { Title = "Blog visits today", Counter = 160, Background = "Black" });
            //myApp.Widgets.Add(new Widget() { Title = "Blog posts", Counter = 12, Background = "Blue" });
            //myApp.Widgets.Add(new Widget() { Title = "Tweets with #helloworld tag", Counter = 65, Background = "Brown" });
            //myApp.Widgets.Add(new Widget() { Title = "Likes today", Counter = 117, Background = "Blue" });
            
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values["NumWidgets"] = Widgets.Count;

            for (int i = 0; i < Widgets.Count; i++)
            {
                var item = Widgets[i];
                localSettings.Values["Widget" + i] = item.Save();

            }
        }
    }
}
