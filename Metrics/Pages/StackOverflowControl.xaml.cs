using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Metrics.Widgets;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Metrics.Pages
{
    public sealed partial class StackOverflowControl : UserControl, IWidget
    {
        public StackOverflowControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals("Reputation"))
            {
                StackOverflowWidget tw = new StackOverflowWidget(Name.Text,Site.Text);
                await tw.Update();
                return tw;
            }
            return null;
        }
    }
}
