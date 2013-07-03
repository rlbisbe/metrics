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
        public enum Selection { Null, Day, Week, Month }

        public WordpressWidget(string blog, string key, Selection selection)
            : this(blog, key)
        {
            this.mSelection = selection;
        }

        public WordpressWidget(string blog, string key, string selection)
            : this(blog, key)
        {
            this.mSelection = (Selection)Enum.Parse(typeof(Selection),
               selection);
        }

        private WordpressWidget(string blog, string key)
        {
            this.Key = key;
            this.Blog = blog;
            if (this.Blog.StartsWith("http://"))
                this.Blog = this.Blog.Substring(7);

            this.Background = "#A3A3A3";
            this.Foreground = "white";
            this.WidgetForeground = "#33ffffff";
            this.WidgetName = "wordpress";
        }

        public string Blog { get; set; }
        public string Key { get; set; }
        public Selection mSelection { get; set; }

        public override async Task Update()
        {
            //Check correct API key:

            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            string result = "";
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            HttpResponseMessage response;
            switch (mSelection)
            {
                case Selection.Day:
                    this.Title = loader.GetString("WordpressWidgetVisitsToday");
                    response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog));
                    result = await response.Content.ReadAsStringAsync();
                    // Parse the JSON recipe data
                    break;
                case Selection.Week:
                    this.Title = loader.GetString("WordpressWidgetVisitsThisWeek");
                    response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog + "&period=week&days=0"));
                    result = await response.Content.ReadAsStringAsync();
                    // Parse the JSON recipe data
                    break;
                case Selection.Month:
                    this.Title = loader.GetString("WordpressWidgetVisitsThisMonth");
                    response = await client.GetAsync(new Uri("http://stats.wordpress.com/csv.php?api_key=" + Key + "&blog_uri=" + Blog + "&period=month&days=0"));
                    result = await response.Content.ReadAsStringAsync();
                    // Parse the JSON recipe data
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
            composite["selection"] = mSelection.ToString();
            return composite;
        }
    }
}
