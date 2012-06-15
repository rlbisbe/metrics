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

        public MainPage()
        {
            this.InitializeComponent();
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
            App myApp = (App)App.Current;
            myApp.HaveInternetConnection();
            this.DefaultViewModel["Items"] = myApp.Widgets;
            if (myApp.Widgets.Count == 0)
            {
                myApp.Widgets.Add(myApp.Empty);
            }

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            object NewLoad = localSettings.Values["NewLoad"];
            if (NewLoad == null)
            {
                MessageDialog msg = new MessageDialog("We have added a few sample widgets to the app, you can delete them and add your own info", "First launch");
                msg.ShowAsync();
            }
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
            if (myApp.HaveInternetConnection() == true)
            {
                foreach (var item in myApp.Widgets)
                {
                    item.Update();
                }
            }
        }
    }
}
