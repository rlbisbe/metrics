using Metrics.Widgets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Metrics
{
    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Group { get; set; }
        public DataTemplate Item { get; set; }
        public DataTemplate Ungrouped { get; set; }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item,
          Windows.UI.Xaml.DependencyObject container)
        {
            App myApp = App.Current as App;
            // unfortunately, our item is an anonymous type because I was lazy and now  
            // I'm being more lazy.  
            dynamic d = (dynamic)item;
            if (myApp.isGrouped)
            {
                return (d is Group ? this.Group : this.Item);
            }
            else
            {
                return this.Ungrouped;
            }
        }
    }
}
