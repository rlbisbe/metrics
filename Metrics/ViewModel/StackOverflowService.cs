using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class StackOverflowService : Service
    {
        public StackOverflowService()
        {
            MetricsProvider = "Stack Overflow";
        }
    }

    public class StackOverflowReputationService : StackOverflowService
    {
        public StackOverflowReputationService()
        {
            MetricsName = "Reputation";
            Properties.Add("User Id", new Value());
        }

        public override async Task<Widget> GetWidget()
        {
            StackOverflowWidget tw = new StackOverflowWidget(
                Properties["User Id"].Content,
                "stackoverflow");
            await tw.Update();
            return tw;
        }
    }
}
