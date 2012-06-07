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
        public StackOverflowWidget(string Source, string Site)
        {
            this.Source = Source;
            this.Site = Site;
            this.Background = "#366fb3";
            this.Foreground = "#ffffff";
        }


        public string Source { get; set; }
        public string Site { get; set; }


        public override async Task Update()
        {
            var client = new GZipHttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024;
            string url = "https://api.stackexchange.com/2.0/users/" + Source + "?site=" + Site;
            var response = await client.GetAsync(new Uri(url));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
            var recipes = JsonObject.Parse(result);
            this.Title = recipes["items"].GetArray()[0].GetObject()["display_name"].GetString() + " reputation on " + Site;
            Counter = (int)recipes["items"].GetArray()[0].GetObject()["reputation"].GetNumber();
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "StackOverflowWidget";
            composite["source"] = Source;
            composite["site"] = Site;
            return composite;
        }
    }
}
