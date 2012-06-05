using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Metrics.Common;
using Windows.Data.Json;
using Windows.Storage;

namespace Metrics.Widgets
{
    class StackOverflowWidget : Widget
    {
        public StackOverflowWidget(string Source)
        {
            this.Source = Source;
            this.Background = "#ffffff";
            this.Foreground = "#ff99dd";
        }


        public string Source { get; set; }


        public override async Task Update()
        {
            var client = new GZipHttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            string url = "https://api.stackexchange.com/2.0/users/" + Source + "?site=stackoverflow";
            var response = await client.GetAsync(new Uri(url));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
            var recipes = JsonObject.Parse(result);
            this.Title = recipes["items"].GetArray()[0].GetObject()["display_name"].GetString() + " reputation";
            Counter = (int)recipes["items"].GetArray()[0].GetObject()["reputation"].GetNumber();
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "StackOverflowWidget";
            composite["source"] = Source;
            return composite;
        }
    }
}
