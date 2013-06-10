using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class FacebookService : Service
    {
        public FacebookService()
        {
            MetricsProvider = "Facebook";
        }
    }

    public class FacebookLikeService : FacebookService
    {
        public FacebookLikeService()
        {
            MetricsName = "Likes";
            Properties.Add("Facebook Page", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            FacebookWidget tw = new FacebookWidget(Properties["Facebook Page"].Content,
                FacebookWidget.Selection.Likes);
            await tw.Update();
            return tw;
        }
    }

    public class FacebookTalkingAboutService : FacebookService
    {
        public FacebookTalkingAboutService()
        {
            MetricsName = "People talking about";
            Properties.Add("Facebook Page", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            FacebookWidget tw 
                = new FacebookWidget(
                    Properties["Facebook Page"].Content,
                FacebookWidget.Selection.TalkingAbout);

            await tw.Update();
            return tw;
        }
    }
}
