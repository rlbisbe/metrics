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
    class FacebookWidget : Widget
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

            this.Source = Source;
            if (this.sel == Selection.Likes)
            {
                this.Title = Source + " likes";
            }
            else if (this.sel == Selection.TalkingAbout)
            {
                this.Title = "people talking about " + Source;
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
            if (this.sel == Selection.Likes)
            {
                this.Title = Source + " likes";
            }
            else if (this.sel == Selection.TalkingAbout)
            {
                this.Title = "people talking about " + Source;
            }
            this.Background = "#385998";
            this.Foreground = "white";
            this.WidgetForeground = "#33ffffff";
            this.WidgetName = "facebook";
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
                throw new NullReferenceException("The selected page was not found. Please check spelling.");
            }
            var result = await response.Content.ReadAsStringAsync();

            // Parse the JSON recipe data
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
