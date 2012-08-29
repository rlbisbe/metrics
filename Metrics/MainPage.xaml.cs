using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Metrics.Widgets;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Metrics.Pages;
// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Metrics
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class MainPage : Metrics.Common.LayoutAwarePage
    {
        double _settingsWidth = 346;

        Popup popup = new Popup();

        public MainPage()
        {
            this.InitializeComponent();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            string privacyStatement = loader.GetString("Privacy");
            SettingsCommand cmd = new SettingsCommand("sample", privacyStatement, (x) =>
            {
                Privacy w = new Privacy(popup);
                w.Width = _settingsWidth;
                w.Height = this.ActualHeight;
                w.Margin = new Thickness(this.ActualWidth - _settingsWidth, 0, 0, 0);
                popup.Child = w;
                popup.IsOpen = true;
                BottomAppBar.IsOpen = false;
            });

            args.Request.ApplicationCommands.Add(cmd);
        }

       
        async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateUI);
        }

        /// <summary>
        /// Checks if there is an internet connection available.
        /// </summary>
        /// 
        /// <returns></returns>
        private bool HaveInternetConnection(string message = "This program requires internet connection for obtaining the metrics data. Please check your internet connection.")
        {
            bool status = true;
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
            {
                status = false;
            }
            else
            {
                var level = profile.GetNetworkConnectivityLevel();
                if (level == NetworkConnectivityLevel.LocalAccess || level == NetworkConnectivityLevel.None)
                {
                    status = false;
                }
            }
            if (status == false)
            {
                ErrorGridText.Text = message;
                ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            return status;
        }

        private void UpdateUI()
        {
            if (HaveInternetConnection(message: "Internet connection was lost"))
            {
                App myApp = (App)App.Current;
                foreach (var item in myApp.Widgets)
                {
                    item.Update();
                }
            }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager,
                DataRequestedEventArgs>(this.OnDataRequested);

            App myApp = (App)App.Current;
            HaveInternetConnection();
            this.DefaultViewModel["Items"] = myApp.Widgets;
            if (myApp.Widgets.Count == 0)
            {
                myApp.Widgets.Add(myApp.Empty);
            }

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            object NewLoad = localSettings.Values["NewLoad"];
            if (NewLoad == null)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                MessageDialog msg = new MessageDialog(loader.GetString("FirstLaunchText"), loader.GetString("FirstLaunchTitle"));
                msg.ShowAsync();
            }
            itemGridView.SelectedItem = null;
            BottomAppBar.IsOpen = false;

            
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

            string dataPackageTitle = loader.GetString("MetricsResume");

            App myApp = (App)App.Current;

            var htmlExample = string.Format(loader.GetString("MetricsResumeContent"), DateTime.Now);

            foreach (var item in myApp.Widgets)
            {
                htmlExample += "<li><i>" + item.WidgetName + "</i> <b>" + item.Title + "</b> " + item.SCounter + "</li>";
            }

            htmlExample += "</ul>";

            var htmlFormat = Windows.ApplicationModel.DataTransfer.HtmlFormatHelper.CreateHtmlFormat(htmlExample);


            DataPackage requestData = args.Request.Data;
            requestData.Properties.Title = dataPackageTitle;

            string dataPackageDescription = "Descripción";

            if (dataPackageDescription != null)
            {
                requestData.Properties.Description = dataPackageDescription;
            }
            requestData.SetHtmlFormat(htmlFormat);


        }

        /// <summary>
        /// Deletes a widget from the system.
        /// </summary>
        private void Delete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App myApp = (App)App.Current;
            var element = itemGridView.SelectedItem;
            if (element != null)
            {
                myApp.Widgets.Remove(element as Widget);
            }
            if (myApp.Widgets.Count == 0)
            {
                myApp.Widgets.Add(myApp.Empty);
            }
            BottomAppBar.IsOpen = false;
        }

        /// <summary>
        /// Adds a new widget.
        /// </summary>
        private void addButton_Click_1(object sender, RoutedEventArgs e)
        {
            AddWidget w = new AddWidget(popup);
            w.Width = this.ActualWidth;
            w.Height = this.ActualHeight;
            popup.Child = w;
            popup.IsOpen = true;
            BottomAppBar.IsOpen = false;
        }

        /// <summary>
        /// Gets the selection from the item grid view.
        /// </summary>
        /// If the selected item is null, disable the button, otherwise
        /// enable the delete button.
        private void itemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (itemGridView.SelectedItem != null)
            {
                deleteButton.Visibility = Visibility.Visible;
                BottomAppBar.IsOpen = true;
            }
            else
            {
                deleteButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Reloads the info from the different widgets.
        /// </summary>
        private void refreshButton_Click_1(object sender, RoutedEventArgs e)
        {
            App myApp = (App)App.Current;
            if (HaveInternetConnection() == true)
            {
                if (itemGridView.SelectedItem != null)
                {
                    (itemGridView.SelectedItem as Widget).Update();
                    return;
                }

                foreach (var item in myApp.Widgets)
                {
                    item.Update();
                }
            }
        }

        public DataTransferManager dataTransferManager { get; set; }

        private void AppBar_Closed_1(object sender, object e)
        {
            itemGridView.SelectedItem = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void MainGrid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (popup.IsOpen == true && popup.Child is Privacy)
            {
                popup.IsOpen = false;
            }
        }
    }
}
