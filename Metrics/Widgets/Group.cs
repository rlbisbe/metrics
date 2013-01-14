using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Widgets
{
    class Group : Widget
    {
        public Type t;
        public Group(Widget w)
        {
            if (w == null)
                return;

            this.WidgetForeground = w.WidgetForeground;
            this.Background = w.Background;
            this.Foreground = w.Foreground;
            this.WidgetName = w.WidgetName;
            this.t = w.GetType();
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (!(obj is Widget))
                return false;
            
            return (obj as Widget).GetType().Equals(t);
        }

        public override Task Update()
        {
            return null;
        }

        public override Windows.Storage.ApplicationDataCompositeValue Save()
        {
            return null;
        }
    }
}
