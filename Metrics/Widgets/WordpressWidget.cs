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
    class WordpressWidget : Widget
    {

        public WordpressWidget(string Blog, string Key, string Selection)
        {
            this.Key = Key;
            this.Blog = Blog;
            if (this.Blog.StartsWith("http://"))
            {
                this.Blog = this.Blog.Substring(7);
            }
            this.Selection = Selection;
            this.Title = Source + " visits";
            this.Background = "#464646";
            this.Foreground = "white";
            this.WidgetForeground = "#33ffffff";
            this.WidgetName = "wordpress";

        }

        public string Source { get; set; }
        public string Blog { get; set; }
        public string Key { get; set; }
        public string Selection { get; set; }

        public override async Task Update()
        {
            //TODO: Check valid syntax of blog, and correct API Key.

            //Check correct API Key:


            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            string result = "";
            switch (Selection)
            {
                case "day":
                    {
                        this.Title = Source + " visits today";
                        var response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog));
                        result = await response.Content.ReadAsStringAsync();
                        // Parse the JSON recipe data
                        break;
                    }
                case "week":
                    {
                        this.Title = Source + " visits this week";
                        var response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog + "&period=week&days=0"));
                        result = await response.Content.ReadAsStringAsync();
                        // Parse the JSON recipe data
                        break;
                    }
                case "month":
                    {
                        this.Title = Source + " visits this month";
                        var response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog + "&period=month&days=0"));
                        result = await response.Content.ReadAsStringAsync();
                        // Parse the JSON recipe data
                        break;
                    }
                default:
                    break;
            }
            if (result.Contains("Error"))
            {
                throw new NullReferenceException(result.Split('\n')[0]);
            }
            Counter = int.Parse(result.Split('\n')[1].Split(',')[1]);
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "WordpressWidget";
            composite["blog"] = Blog;
            composite["key"] = Key;
            composite["selection"] = Selection;
            return composite;
        }


    }
}
