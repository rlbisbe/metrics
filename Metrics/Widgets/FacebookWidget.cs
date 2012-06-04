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
    class FacebookWidget: Widget
    {
        public FacebookWidget(string Source)
        {
            this.Source = Source;
            this.Title = Source + " likes";
            this.Background = "#385998";
            this.Foreground = "white";
        }

        public string Source { get; set; }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("https://graph.facebook.com/" + Source));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
            var recipes = JsonObject.Parse(result);

            Counter = (int)recipes["likes"].GetNumber();
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "FacebookWidget";
            composite["source"] = Source;
            return composite;
        }
    }
}
