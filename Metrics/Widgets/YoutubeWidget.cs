using Metrics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Widgets
{
    class YoutubeWidget : Widget
    {
        public YoutubeWidget(string source)
        {
            SetUrl(source);
            SetTexts();
            SetColors();
        }

        private void SetUrl(string source)
        {
            this.Source = source.Split('=')[1];
        }

        private void SetTexts()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            this.WidgetName = "youtube";
        }

        private void SetColors()
        {
            this.Background = "#d02525";
            this.Foreground = "black";
            this.WidgetForeground = "#33000000";
        }

        public override async Task Update()
        {
            var result = await HttpService.
                GetJsonResult("https://gdata.youtube.com/feeds/api/videos/"
                + this.Source + "?v=2&alt=jsonc");
            Counter = (int)result["data"].GetObject()["viewCount"].GetNumber();
            Title = result["data"].GetObject()["title"].GetString();
        }

        public string Source { get; set; }

        public override Windows.Storage.ApplicationDataCompositeValue Save()
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
