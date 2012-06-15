using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Metrics.Widgets;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Popups;
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
        public EmptyWidget Empty;

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
            Empty = new EmptyWidget();
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
                    if (w != null)
                    {
                        w.Update();
                        this.Widgets.Add(w);
                    }
                }
            }

            object NewLoad = localSettings.Values["NewLoad"];
            if (NewLoad == null)
            {
                Widget w;

                w = new FacebookWidget("cocacola",FacebookWidget.Selection.Likes);
                w.Update(); 
                this.Widgets.Add(w);

                w = new TweetWidget("windows");
                w.Update(); 
                this.Widgets.Add(w);

                w = new GithubWidget("twitter", "bootstrap");
                w.Update();
                this.Widgets.Add(w);

                w = new FacebookWidget("microsoft", FacebookWidget.Selection.TalkingAbout);
                w.Update(); 
                this.Widgets.Add(w);

                w = new TuentiWidget("cocacola");
                w.Update(); 
                this.Widgets.Add(w);

                w = new TweetWidget("microsoft");
                w.Update();
                this.Widgets.Add(w);

                w = new StackOverflowWidget("190165","stackoverflow");
                w.Update();
                this.Widgets.Add(w);

                w = new GithubWidget("rails", "rails");
                w.Update();
                this.Widgets.Add(w);
            
            }

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            rootFrame.Navigate(typeof(MainPage));

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            
        }

        /// <summary>
        /// Checks if there is an internet connection available.
        /// </summary>
        /// <returns></returns>
        public bool HaveInternetConnection()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
            {
                MessageDialog msg = new MessageDialog("This program requires internet connection for obtaining the metrics data. Please check your internet connection.", "Internet connection not found.");
                msg.ShowAsync();
                return false;
            }
            else
            {
                var level = profile.GetNetworkConnectivityLevel();
                if (level == NetworkConnectivityLevel.LocalAccess || level == NetworkConnectivityLevel.None)
                {
                    MessageDialog msg = new MessageDialog("This program requires internet connection for obtaining the metrics data. Please check your internet connection.", "Internet connection not found.");
                    msg.ShowAsync();
                    return false;
                }
            }
            return true;
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

            localSettings.Values["NewLoad"] = true;
        }
    }
}
