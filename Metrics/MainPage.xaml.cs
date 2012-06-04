using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the collection of items to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App myApp = (App)App.Current;
            this.DefaultViewModel["Items"] = myApp.Widgets;
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
            foreach (var item in myApp.Widgets)
            {
                item.Update();
            }
        }
    }
}
