using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.ViewModel
{
    public abstract class WordpressService : Service
    {
        public WordpressService()
        {
            MetricsProvider = "Wordpress";
            Properties.Add("URL", new Value());
            Properties.Add("Key", new Value());
        }  
    }

    public class WordpressDayService : WordpressService
    {
        public WordpressDayService()
        {
            MetricsName = "Day";
        }

        public override async Task<Widget> GetWidget()
        {
            WordpressWidget tw = new WordpressWidget(
                Properties["URL"].Content,
                Properties["Key"].Content,
                WordpressWidget.Selection.Day);
            await tw.Update();
            return tw;
        }
    }

    public class WordpressWeekService : WordpressService
    {
        public WordpressWeekService()
        {
            MetricsName = "Week";
        }

        public override async Task<Widget> GetWidget()
        {
            WordpressWidget tw = new WordpressWidget(
                Properties["URL"].Content,
                Properties["Key"].Content, 
                WordpressWidget.Selection.Week);
            await tw.Update();
            return tw;
        }
    }

    public class WordpressMonthService : WordpressService
    {
        public WordpressMonthService()
        {
            MetricsName = "Month";
        }

        public override async Task<Widget> GetWidget()
        {
            WordpressWidget tw = new WordpressWidget(
                Properties["URL"].Content,
                Properties["Key"].Content,
                WordpressWidget.Selection.Month);
            await tw.Update();
            return tw;
        }
    }
}
