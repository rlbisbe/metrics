using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class RedditService : Service
    {
        public RedditService()
        {
            MetricsProvider = "Reddit";
        }
    }

    public class RedditCommentsService : RedditService 
    {
        public RedditCommentsService()
        {
            MetricsName = "Comments";
            Properties.Add("URL", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            RedditWidget rw = new RedditWidget(
                Properties["URL"].Content,
                RedditMetricType.Comments);
            await rw.Update();
            return rw;
        }
    }

    public class RedditScoreService : RedditService
    {
        public RedditScoreService()
        {
            MetricsName = "Score";
            Properties.Add("URL", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            RedditWidget rw = new RedditWidget(
                Properties["URL"].Content,
                RedditMetricType.Score);
            await rw.Update();
            return rw;
        }
    }

}
