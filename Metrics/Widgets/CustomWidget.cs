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
    /// Custom widget, for adding specific web services
    /// </summary>
    class CustomWidget : Widget
    {
        public CustomWidget(string url, string data, string widgetName, string title)
        {
            this.Background = "blue";
            this.Foreground = "white";
            this.WidgetForeground = "#33ffffff";
            this.WidgetName = widgetName;
            this.Data = data;
            this.URL = url;
            this.Title = title;
        }

        public string URL { get; set; }
        public string Data { get; set; }

        public override async Task Update()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri(URL));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                throw new NullReferenceException(loader.GetString("ErrorFBControlPageNotFound"));
            }
            var result = await response.Content.ReadAsStringAsync();
            // Parse the JSON recipe data
            var recipes = JsonObject.Parse(result);
            Counter = (int)recipes[Data].GetNumber();
        }

        public override ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "CustomWidget";
            composite["widgetName"] = WidgetName;
            composite["url"] = URL;
            composite["data"] = Data;
            composite["title"] = Title;
            return composite;
        }
    }
}
