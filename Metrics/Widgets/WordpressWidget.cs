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
    class WordpressWidget: Widget
    {
        public WordpressWidget(string Blog, string Key)
        {
            this.Key = Key;
            this.Blog = Blog;
            this.Title = Source + " visits today";
            this.Background = "#464646";
            this.Foreground = "white";
            this.WidgetForeground = "#338ec9e8";
            this.WidgetName = "wordpress";
        }

        public string Source { get; set; }
        public string Blog { get; set; }
        public string Key { get; set; }

        public override async Task Update()
        {
            //TODO: Check valid syntax of blog, and correct API Key.


            // For last day: http://stats.wordpress.com/csv.php?api_key=9bb99caddf84&blog_uri=robertoluis.wordpress.com&days=1 -< Yesterday: Today = 0
            // This week: http://stats.wordpress.com/csv.php?api_key=9bb99caddf84&blog_uri=robertoluis.wordpress.com&period=week&days=0
            // This month: http://stats.wordpress.com/csv.php?api_key=9bb99caddf84&blog_uri=robertoluis.wordpress.com&period=month&days=0
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog));
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data

            Counter = int.Parse(result.Split('\n')[1].Split(',')[1]);
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "WordpressWidget";
            composite["blog"] = Blog;
            composite["key"] = Key;
            return composite;
        }
    }
}
