using Metrics.Widgets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Metrics
{
    public class MyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Group { get; set; }
        public DataTemplate Item { get; set; }
        public DataTemplate Ad { get; set; }
        public DataTemplate Ungrouped { get; set; }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item,
          Windows.UI.Xaml.DependencyObject container)
        {
            App myApp = App.Current as App;
            // unfortunately, our item is an anonymous type because I was lazy and now  
            // I'm being more lazy.  
            dynamic d = (dynamic)item;
            if (myApp.IsGrouped)
            {
                if (d is Group)
                    return this.Group;

                if (d is Widget)
                    return this.Item;

                if (d is AdWidget)
                    return this.Ad;

                return null;
            }
            else
            {
                if (d is AdWidget)
                    return this.Ad;

                if (d is Widget)
                    return this.Ungrouped;

                return null;
            }
        }
    }
}
