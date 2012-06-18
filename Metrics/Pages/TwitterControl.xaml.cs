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
    public sealed partial class TwitterControl : UserControl, IWidget
    {
        public int Tweets { get; set; }

        public TwitterControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            if (String.IsNullOrEmpty(Name.Text))
            {
                throw new NullReferenceException("Twitter handle cannot be null");
            }
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals(ControlFollowers.Content))
            {
                TweetWidget tw = new TweetWidget(Name.Text);
                await tw.Update();
                return tw;
            }
            return null;
        }
    }
}
