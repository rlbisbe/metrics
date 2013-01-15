using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Metrics.Widgets;
using Windows.Foundation;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Automation;
using Metrics.ViewModel;
using Metrics.Services;
using Microsoft.Advertising.WinRT.UI;
// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Metrics
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class MainPage : Metrics.Common.LayoutAwarePage
    {
        Popup popup = new Popup();
        private CoreDispatcher mDispatcher;
        private bool mHaveConnection;
        private App myApp = App.Current as App;

        public MainPage()
        {
            this.InitializeComponent();
            mDispatcher = Window.Current.Dispatcher;
            mHaveConnection = NetworkInformation.GetInternetConnectionProfile() != null;
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            this.DataContext = new AppViewModel();
        }

        async void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (NetworkInformation.GetInternetConnectionProfile() == null)
            {
                if (mHaveConnection)
                {
                    mHaveConnection = false;
                    //Código a ejecutar cuando se pierda la conexión
                    await mDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ErrorGridText.Text =
                                LocalizationService.GetString("LostInternetConnection");
                            ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                        });
                }
            }
            else
            {
                if (!mHaveConnection)
                {
                    mHaveConnection = true;
                    //Código a ejecutar cuando se recupere la conexión
                    await mDispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ErrorGridText.Text = LocalizationService.GetString("RecoveredInternetConnection");
                            ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                            (this.DataContext as AppViewModel).RefreshCommand.Execute(null);
                        });
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

            if (this.DataContext as AppViewModel == null)
                return;
            
            (this.DataContext as AppViewModel).RefreshCommand.Execute(null);

            if (myApp.Widgets.Count == 0)
                myApp.Widgets.Add(myApp.Empty);

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            object NewLoad = localSettings.Values["NewLoad"];
            if (NewLoad == null)
            {
                MessageDialog msg = new MessageDialog(
                    LocalizationService.GetString("FirstLaunchText"),
                    LocalizationService.GetString("FirstLaunchTitle"));
                msg.ShowAsync();
            }
            itemGridView.SelectedItem = null;
            BottomAppBar.IsOpen = false;

            //itemListView.Items.Add(new AdControl() {
            //    ApplicationId = "5631da7f-12d0-4c10-ad98-3927d4b32ab8",
            //    AdUnitId = "112816"
            //});
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            string dataPackageTitle = LocalizationService.GetString("MetricsResume");
            var htmlExample = string.Format(
                LocalizationService.GetString("MetricsResumeContent"),
                DateTime.Now);

            IEnumerable<Widget> widgets =
                this.DefaultViewModel["Items"] as IEnumerable<Widget>;
            foreach (var item in widgets)
            {
                if (item is Group)
                {
                    htmlExample += string.Format("</ul><li><random>{0}</random><ul>",
                        item.WidgetName);
                }
                else
                {
                    htmlExample += string.Format("<li>{0}<b>{1}</b></li>",
                        item.SCounter,
                        item.Title);
                }
            }

            htmlExample += "</ul>";

            var htmlFormat = HtmlFormatHelper.CreateHtmlFormat(htmlExample);

            DataPackage requestData = args.Request.Data;
            requestData.Properties.Title = dataPackageTitle;
            requestData.Properties.Description = "Descripción";
            requestData.SetHtmlFormat(htmlFormat);
        }

        /// <summary>
        /// Deletes a widget from the system.
        /// </summary>
        private void Delete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
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
            AppViewModel viewModel = this.DataContext as AppViewModel;

            AddWidget w = new AddWidget(popup, viewModel);
            w.Width = this.ActualWidth;
            w.Height = this.ActualHeight;
            popup.Child = w;
            popup.IsOpen = true;
            BottomAppBar.IsOpen = false;
            viewModel.SwitchAd();
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

        public DataTransferManager dataTransferManager { get; set; }

        private void AppBar_Closed_1(object sender, object e)
        {
            itemGridView.SelectedItem = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Button_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            AppViewModel viewModel = this.DataContext as AppViewModel;
            viewModel.IsGrouped = !viewModel.IsGrouped;

            if (viewModel.IsGrouped)
            {
                groupButton.Content = "\uE0F4";
                groupButton.SetValue(AutomationProperties.NameProperty, loader.GetString("UngroupText"));
                return;

            }
            groupButton.Content = "\uE15C";
            groupButton.SetValue(AutomationProperties.NameProperty,
                LocalizationService.GetString("GroupText"));
        }

        private void ShowGrouped()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            AppViewModel viewModel = this.DataContext as AppViewModel;
            viewModel.GroupItems();
            myApp.IsGrouped = true;
            groupButton.Content = "\uE0F4";
            groupButton.SetValue(AutomationProperties.NameProperty, loader.GetString("UngroupText"));
        }
    }
}
