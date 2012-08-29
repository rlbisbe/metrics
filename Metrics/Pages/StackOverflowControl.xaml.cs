using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Metrics.Common;
using Metrics.Widgets;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Metrics.Pages
{
    public sealed partial class StackOverflowControl : UserControl, IWidget
    {
        class MyComboBoxItem : ComboBoxItem
        {
            public string URL { get; set; }
            public string API_ID { get; set; }
        }

        public StackOverflowControl()
        {
            this.InitializeComponent();
            this.LoadData();
        }

        async private void LoadData()
        {
            int length = 10;
            var client = new GZipHttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;
            string url = "https://api.stackexchange.com/2.0/sites?pagesize=999";
            var response = await client.GetAsync(new Uri(url));
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new NullReferenceException("The selected site was not found. Please check spelling.");
            }

            var result = await response.Content.ReadAsStringAsync();
            var recipes = JsonObject.Parse(result);
            var array = recipes["items"].GetArray();
            if (array.Count == 0)
            {
                throw new NullReferenceException("The user id was not found.");
            }
            foreach (var item in array)
            {
                var fg = item.GetObject()["styling"].GetObject()["tag_foreground_color"].GetString();
                var bg = item.GetObject()["styling"].GetObject()["tag_background_color"].GetString();
                StackSite.Items.Add(new MyComboBoxItem()
                {
                    Content = item.GetObject()["name"].GetString(),
                    API_ID = item.GetObject()["api_site_parameter"].GetString(),
                    URL = item.GetObject()["site_url"].GetString(),
                    Foreground = new SolidColorBrush(HexColor(fg)),
                    Background = new SolidColorBrush(HexColor(bg))
                    //item.GetObject()["styling"].GetObject()["tag_foreground_color"]
                });
            }
        }

        public Color HexColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                start = 2;
            }

            else if (hex.Length == 3)
            {
                string rs = hex.Substring(start, 1) + hex.Substring(start, 1);
                r = byte.Parse(rs, NumberStyles.HexNumber);
                string gs = hex.Substring(start + 1, 1) + hex.Substring(start + 1, 1);
                g = byte.Parse(gs, NumberStyles.HexNumber);
                string bs = hex.Substring(start + 2, 1) + hex.Substring(start + 2, 1);
                b = byte.Parse(bs, NumberStyles.HexNumber);
            }
            else
            {
                //convert RGB characters to bytes
                r = byte.Parse(hex.Substring(start, 2), NumberStyles.HexNumber);
                g = byte.Parse(hex.Substring(start + 2, 2), NumberStyles.HexNumber);
                b = byte.Parse(hex.Substring(start + 4, 2), NumberStyles.HexNumber);
            }
           

            return Color.FromArgb(a, r, g, b);
        }

        async public Task<Widget> GetWidget()
        {
            UserIDError.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SelectedItemError.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bool error = false;
            if (String.IsNullOrEmpty(Name.Text))
            {
                UserIDError.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (StackSite.SelectedItem == null)
            {
                SelectedItemError.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (error == true)
            {
                return null;
            }
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals(Reputation.Content))
            {
                StackOverflowWidget tw = new StackOverflowWidget(Name.Text,(StackSite.SelectedItem as MyComboBoxItem).API_ID);
                await tw.Update();
                return tw;
            }
            return null;
        }

        private void StackSite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            UserID.Text = String.Format(loader.GetString("StackOverflowControlURL"), (StackSite.SelectedItem as MyComboBoxItem).URL);
        }
    }
}
