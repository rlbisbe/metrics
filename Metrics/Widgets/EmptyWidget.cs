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
            this.Title = "For adding a new widget click on the + sign.";
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
