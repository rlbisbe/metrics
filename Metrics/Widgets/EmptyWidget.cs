using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Metrics.Widgets
{
    public class EmptyWidget : Widget
    {
        public EmptyWidget()
        {
            this.WidgetName = "EmptyWidget";
            this.Foreground = "white";
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            this.Title = loader.GetString("EmptyWidgetTitle");
        }

        public override async Task Update()
        {
            return;
        }

        public override ApplicationDataCompositeValue Save()
        {
            return null;
        }
    }
}
