using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Metrics.Widgets;
using Windows.Data.Json;
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
    public sealed partial class WordpressControl : UserControl, IWidget
    {
        public int Tweets { get; set; }

        public WordpressControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            ErrorNullApiKey.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ErrorBlogUrl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bool error = false;
            if (String.IsNullOrEmpty(API.Text))
            {
                ErrorNullApiKey.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (String.IsNullOrEmpty(URL.Text))
            {
                ErrorBlogUrl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (error == true)
            {
                return null;
            }
            WordpressWidget tw;
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("Visits today"))
            {
                tw = new WordpressWidget(URL.Text,API.Text,"day");

            } else if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("Visits this week"))
            {
                tw = new WordpressWidget(URL.Text, API.Text, "week");

            } else if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("Visits this month"))
            {
                tw = new WordpressWidget(URL.Text, API.Text, "month");
            }
            else
            {
                return null;
            }
            await tw.Update();
            return tw;
        }
    }
}
