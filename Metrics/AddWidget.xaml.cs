using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Metrics.Pages;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Metrics
{
    public sealed partial class AddWidget : UserControl
    {
        public AddWidget(Popup popup, ViewModel.AppViewModel viewModel)
        {
            this.popup = popup;
            this.InitializeComponent();
            ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.viewModel = viewModel;
        }

        private void Close(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            popup.IsOpen = false;
            viewModel.SwitchAd();
        }

        async private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var widget = await (WidgetContainer.Children[0] as IWidget).GetWidget();
            if (widget == null)
                return;

            try
            {
                viewModel.Add(widget);
                popup.IsOpen = false;
                viewModel.SwitchAd();
            }
            catch (Exception ex)
            {
                ErrorGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ErrorGridText.Text = "Error: " + ex.Message;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string service = (e.OriginalSource as Button).Content.ToString();
            WidgetContainer.Children.Clear();
            switch (service)
            {
                case "Facebook":
                    WidgetContainer.Children.Add(new FacebookControl());
                    break;
                case "Github":
                    WidgetContainer.Children.Add(new GithubControl());
                    break;
                case "StackOverflow":
                    WidgetContainer.Children.Add(new StackOverflowControl());
                    break;
                case "Tuenti":
                    WidgetContainer.Children.Add(new TuentiControl());
                    break;
                case "Twitter":
                    WidgetContainer.Children.Add(new TwitterControl());
                    break;
                case "Wordpress":
                    WidgetContainer.Children.Add(new WordpressControl());
                    break;
                case "Youtube":
                    WidgetContainer.Children.Add(new YoutubeControl());
                    break;
                default:
                    break;
            }
            //if (service == CustomWidget.Content.ToString())
            //    WidgetContainer.Children.Add(new CustomWidgetControl());
            myStoryboard.Begin();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ErrorGrid.Visibility = Visibility.Collapsed;
        }

        private Popup popup;
        private ViewModel.AppViewModel viewModel;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Content.Equals("Facebook"))
            {
                WidgetContainer.Children.Add(new FacebookControl());   
            }
        }
    }
}
