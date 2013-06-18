using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class GithubService : Service
    {
        public GithubService()
        {
            MetricsProvider = "Github";
        }
    }

    public class GithubIssuesService : GithubService
    {
        public GithubIssuesService()
        {
            MetricsName = "Issues";
            Properties.Add("Username", new Value());
            Properties.Add("Repository", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            GithubWidget wid = new GithubWidget(
                Properties["Username"].Content,
                Properties["Repository"].Content);
            await wid.Update();
            return wid;
        }
    }
}
