using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Metrics.ViewModel
{
    public abstract class TwitterService : Service 
    {
        public TwitterService()
        {
            MetricsProvider = "Twitter";
        }
    }

    public class TwitterFollowersService : TwitterService
    {
        public TwitterFollowersService()
        {
            MetricsName = "Followers";
            Properties.Add("Twitter Account", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            TweetWidget tw = new TweetWidget(Properties["Twitter Account"].Content);
            await tw.Update();
            return tw;
        }
    }
}
