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
    public sealed partial class YoutubeControl : UserControl, IWidget
    {
        public int Tweets { get; set; }

        public YoutubeControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            if (String.IsNullOrEmpty(Name.Text))
            {
                PageError.Text = loader.GetString("ErrorFBControlPageNull");
                PageError.Visibility = Windows.UI.Xaml.Visibility.Visible;
                return null;
            }
            else
            {
                PageError.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            
            YoutubeWidget tw = new YoutubeWidget(Name.Text);
            
            await tw.Update();
            return tw;
        }
    }
}
