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
    class TweetWidget : Widget
    {
        public TweetWidget(string Source)
        {
            this.Source = Source;
            this.Title =  Source + " followers";
            this.Background = "#33CCFF";
            this.Foreground = "black";
        }

        public string Source { get; set; }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("https://api.twitter.com/1/users/show.json?screen_name=" + Source));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
            var recipes = JsonObject.Parse(result);

            Counter = (int)recipes["followers_count"].GetNumber();
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
