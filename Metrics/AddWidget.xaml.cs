using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Metrics.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        Popup p;
        public AddWidget(Popup p)
        {
            this.p = p;
            this.InitializeComponent();
        }

        private void Submit_Click_1(object sender, RoutedEventArgs e)
        {  
            p.IsOpen = false;
        }

        private void Button_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
			p.IsOpen = false;
        	// TODO: Add event handler implementation here.
        }

        async private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Submit
            App myApp = (App)App.Current;
            p.IsOpen = false;
            var w = await (WidgetContainer.Children[0] as IWidget).GetWidget();
            myApp.Widgets.Add(w);
        }

        private void ComboBox_SelectionChanged_1(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            string service = (Services.SelectedItem as ComboBoxItem).Content.ToString();
            switch (service)
            {
                case "facebook":
                    WidgetContainer.Children.Clear();
                    WidgetContainer.Children.Add(new FacebookControl());
                    break;
                case "Tuenti":
                    WidgetContainer.Children.Clear();
                    WidgetContainer.Children.Add(new TuentiControl());
                    break;
                case "Twitter":
                    WidgetContainer.Children.Clear();
                    WidgetContainer.Children.Add(new TwitterControl());
                    break;
                case "Wordpress":
                    WidgetContainer.Children.Clear();
                    WidgetContainer.Children.Add(new WordpressControl());
                    break;
                
                default:
                    break;
            }
        }
    }
}
