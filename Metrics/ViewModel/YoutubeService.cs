using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class YoutubeService : Service
    {
        public YoutubeService()
        {
            MetricsProvider = "Youtube";
        }
    }

    public class YoutubeViewsService : YoutubeService
    {
        public YoutubeViewsService()
        {
            MetricsName = "Views";
            Properties.Add("URL", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            YoutubeWidget tw = new YoutubeWidget(
                Properties["URL"].Content,
                YoutubeWidget.Selection.ViewCount);
            await tw.Update();
            return tw;
        }
    }

    public class YoutubeUpvotesService : YoutubeService
    {
        public YoutubeUpvotesService()
        {
            MetricsName = "Like count";
            Properties.Add("URL", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            YoutubeWidget tw = new YoutubeWidget(
                Properties["URL"].Content,
                YoutubeWidget.Selection.LikeCount);
            await tw.Update();
            return tw;
        }
    }
}
