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
using Metrics.ViewModel;
using Windows.UI;
using Windows.UI.Text;

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

            ServicesComboBox.ItemsSource = viewModel.Services;
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
                case "StackOverflow":
                    WidgetContainer.Children.Add(new StackOverflowControl());
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
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            ErrorGrid.Visibility = Visibility.Collapsed;
        }

        private Popup popup;
        private ViewModel.AppViewModel viewModel;

        private void ComboBox_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            Service currentService
                = ServicesComboBox.SelectedItem as Service;

            metricDetails.Children.Clear();

            foreach (string item in currentService.Properties.Keys)
            {
                var textBlock = new TextBlock();
                textBlock.Text = item;
                textBlock.Style = App.Current.Resources["TitleTextStyle"] as Style;
                textBlock.Margin = new Thickness(0, 16, 0, 0);
                metricDetails.Children.Add(textBlock);

                var textBox = new TextBox();
                textBox.Foreground = new SolidColorBrush(Colors.Black);
                textBox.Margin = new Thickness(0, 8, 0, 0);

                Binding binding = new Binding();
                binding.Path = new PropertyPath("Content");
                binding.Source = currentService.Properties[item];
                binding.Mode = BindingMode.TwoWay;
                textBox.SetBinding(TextBox.TextProperty, binding);
                metricDetails.Children.Add(textBox);
            }
        }

        private async void addMetric_Click(object sender, RoutedEventArgs e)
        {
            Service currentService
                = ServicesComboBox.SelectedItem as Service;

            var widget = await currentService.GetWidget();
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
    }
}
