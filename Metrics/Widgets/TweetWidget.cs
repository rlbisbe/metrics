using Metrics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Metrics.Widgets
{
    public class TweetWidget : Widget
    {
        public TweetWidget(string source)
        {
            SetTwitterUsername(source);
            SetColors();
            SetTexts();
        }

        private void SetTexts()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            this.Title = String.Format(loader.GetString("TWWidgetFollowers"), this.Source);
            this.WidgetName = "twitter";
        }

        private void SetColors()
        {
            this.Background = "#33CCFF";
            this.Foreground = "black";
            this.WidgetForeground = "#33000000";
        }

        private void SetTwitterUsername(string source)
        {
            if (source.Contains("@"))
            {
                this.Source = source.Substring(1);
                return;
            }

            this.Source = source;
        }

        public string Source { get; set; }

        public override async Task Update()
        {
            var result = await HttpService.
                GetJsonResult("https://api.twitter.com/1/users/show.json?screen_name="
                + this.Source);
            Counter = (int)result["followers_count"].GetNumber();
        }

        /// <summary>
        /// Converts the content of the widget into an ApplicationDataCompositeValue,
        /// for saving it in the local system.
        /// </summary>
        /// <returns></returns>
        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "TweetWidget";
            composite["source"] = Source;
            return composite;
        }
    }
}
