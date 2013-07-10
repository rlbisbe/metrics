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
    public enum RedditMetricType { Score, Comments }
    
    /// <summary>
    /// Reddit Widget
    /// </summary>
    public class RedditWidget : Widget
    {
        public string Url { get; set; }
        public RedditMetricType MetricType { get; set; }

        public RedditWidget(string url, RedditMetricType type)
        {
            this.Url = url;
            this.MetricType = type;
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            this.Title = "Reddit stuff";
            this.Background = "#ff4500";
            this.Foreground = "black";
            this.WidgetForeground = "#33000000";
            this.WidgetName = "reddit";
        }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri(Url + ".json"));
                var result = await response.Content.ReadAsStringAsync();

            var recipes = JsonArray.Parse(result);
            var element = recipes
                .GetArray()[0]
                .GetObject()["data"]
                .GetObject()["children"]
                .GetArray()[0]
                .GetObject()["data"]
                .GetObject();

            switch (MetricType)
            {
                case RedditMetricType.Score:
                    Counter = (int)element["score"].GetNumber();
                    break;
                case RedditMetricType.Comments:
                    Counter = (int)element["num_comments"].GetNumber();
                    break;
                default:
                    break;
            }

        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "RedditWidget";
            return composite;
        }
    }
}
