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
    public sealed partial class FacebookControl : UserControl, IWidget
    {
        public int Tweets { get; set; }

        public FacebookControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            if (String.IsNullOrEmpty(Name.Text))
            {
                throw new NullReferenceException("Page cannot be null");
            }
            FacebookWidget tw;
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("Page likes"))
            {
                tw = new FacebookWidget(Name.Text, FacebookWidget.Selection.Likes);
            }
            else if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("People talking about count"))
            {
                tw = new FacebookWidget(Name.Text, FacebookWidget.Selection.TalkingAbout);
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
