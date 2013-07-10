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
    /// <summary>
    /// Facebook Widget.
    /// </summary>
    /// API Info: http://developers.facebook.com/docs/reference/api/
    public class FacebookWidget : Widget
    {
        public enum Selection { Null, Likes, TalkingAbout }

        public FacebookWidget(string Source, string selection)
        {
            this.Source = Source;
            if (selection.Equals("Likes"))
            {
                this.sel = Selection.Likes;
            }
            else if (selection.Equals("TalkingAbout"))
            {
                this.sel = Selection.TalkingAbout;
            }
            init();
        }

        /// <summary>
        /// Initializes the widget, specifying the page name and extracting it from the source.
        /// </summary>
        private void init()
        {
            if (this.Source.Contains("http://"))
            {
                this.Source = this.Source.Substring(Source.IndexOf("//") + 2);
            }
            if (this.Source.Contains("facebook.com"))
            {
                this.Source = this.Source.Substring(Source.IndexOf("/") + 1);
            }

            if (this.sel == Selection.Likes)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                this.Title = String.Format(loader.GetString("FBWidgetLikes"), Source);
            }
            else if (this.sel == Selection.TalkingAbout)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                this.Title = String.Format(loader.GetString("FBWidgetPeopleTalkingAbout"), Source);
            }
            this.Background = "#385998";
            this.Foreground = "white";
            this.WidgetForeground = "#33ffffff";
            this.WidgetName = "facebook";
        }

        public FacebookWidget(string Source, Selection s)
        {
            this.sel = s;
            this.Source = Source;
            init();
        }

        public string Source { get; set; }
        public Selection sel { get; set; }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri("https://graph.facebook.com/" + Source));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                throw new NullReferenceException(loader.GetString("ErrorFBControlPageNotFound"));
            }

            var result = await response.Content.ReadAsStringAsync();

            if (result == "false")
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                throw new NullReferenceException(loader.GetString("ErrorFBControlPageNotFound"));
            }

            // Parse the JSON recipe data
            try
            {
                var recipes = JsonObject.Parse(result);
                if (sel == Selection.TalkingAbout)
                {
                    Counter = (int)recipes["talking_about_count"].GetNumber();
                }
                else if (sel == Selection.Likes)
                {
                    Counter = (int)recipes["likes"].GetNumber();
                }
            }
            catch (Exception)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                throw new NullReferenceException(loader.GetString("ErrorFBControlPageNotFound"));
            }
            

        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "FacebookWidget";
            composite["source"] = Source;
            composite["selection"] = sel.ToString();
            return composite;
        }
    }
}
