using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override Windows.Storage.ApplicationDataCompositeValue Save()
        {
            return null;
        }
    }
}
