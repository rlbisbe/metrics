using Metrics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace Metrics.Library.Widgets
{
    public class YoutubeWidget : Widget
    {
        private Selection mSelection;
        public enum Selection { Null, ViewCount, LikeCount }

        public YoutubeWidget(string source, string selection) 
            : this(source)
        {
            this.mSelection = (Selection)Enum.Parse(typeof(Selection),
                selection);
        }

        public YoutubeWidget(string source, 
            Selection selection) 
            : this(source)
        {
            this.mSelection = selection;
            HttpService = new HttpService();
        }

        private YoutubeWidget (string source)
	    {
            SetUrl(source);
            SetTexts();
            SetColors();
	    }

        private void SetUrl(string source)
        {
            string[] result = source.Split('=');
            
            if (result.Length != 2)
            {
                this.Source = source;
                return;
            }

            this.Source = result[1];
        }

        private void SetTexts()
        {
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

            switch (mSelection)
            {
                case Selection.ViewCount:
                    Counter = (int)result["data"].GetObject()["viewCount"].GetNumber();
                    break;
                case Selection.LikeCount:
                    var str = result["data"].GetObject()["likeCount"].GetString();
                    Counter = int.Parse(str);                    
                    break;
            }
            Title = result["data"].GetObject()["title"].GetString();
        }

        public string Source { get; set; }

        public override Windows.Storage.ApplicationDataCompositeValue Save()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["name"] = "YoutubeWidget";
            composite["url"] = Source;
            composite["selection"] = mSelection.ToString();
            return composite;
        }

        public IHttpService HttpService { get; set; }
    }
}
