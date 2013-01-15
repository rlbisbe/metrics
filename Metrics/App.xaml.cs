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
using Windows.UI.ApplicationSettings;
using Windows.System;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Metrics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private bool m_settingsReady = false;
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
            // Notifícame cuando el usuario abra el panel de Settings
            if (!this.m_settingsReady)
            {
                SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
                this.m_settingsReady = true;
            }

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

                w = new FacebookWidget("cocacola", FacebookWidget.Selection.Likes);
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

                w = new StackOverflowWidget("190165", "stackoverflow");
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
            Window.Current.VisibilityChanged += Current_VisibilityChanged;
        }

        void OnCommandsRequested(
            SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            // Preparamos el metodo que se llamará cuando el usuario pulse 
            // en alguno de los Settings
            UICommandInvokedHandler handler = new UICommandInvokedHandler(OnSettingsCommand);

            // Hacemos que las diferentes opciones aparezcan en los Settings

            // Política de privacidad
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string privacyStatement = loader.GetString("Privacy");
            SettingsCommand privacyPolicyCommand =
                new SettingsCommand("politicaPrivacidad", privacyStatement, handler);
            eventArgs.Request.ApplicationCommands.Add(privacyPolicyCommand);

            // Contacta con nosotros
            string contactUs = loader.GetString("ContactUs");
            SettingsCommand contactUsCommand =
                new SettingsCommand("contactaConNosotros", contactUs, handler);
            eventArgs.Request.ApplicationCommands.Add(contactUsCommand);
        }

        async void OnSettingsCommand(IUICommand command)
        {
            // Obtenemos en cuál de los Setting ha pulsado el usuario
            SettingsCommand settingsCommand = (SettingsCommand)command;

            // Según el que haya pulsado hacemos unas cosas u otras
            switch ((string)settingsCommand.Id)
            {
                case "politicaPrivacidad":

                    // Abrimos la página web con nuestra Política de Privacidad 
                    // (nota: el enlace es de ejemplo, aquí deberías de poner el tuyo propio)
                    await Launcher.LaunchUriAsync(new Uri(
                        "http://apps.rlbisbe.net/metrics/privacy"));

                    break;

                case "contactaConNosotros":

                    // Abrimos la página web con nuestra información de contacto
                    // (nota: el enlace es de ejemplo, aquí deberías de poner el tuyo propio)
                    await Launcher.LaunchUriAsync(new Uri(
                        "http://robertoluis.wordpress.com/contacto/"));

                    break;
            }
        }

        void Current_VisibilityChanged(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
        {
            if (!e.Visible)
            {
                OnSuspending(sender,null);
            } 
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

        public bool IsGrouped { get; set; }
    }
}
