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
    public sealed partial class CustomWidgetControl : UserControl, IWidget
    {
        public CustomWidgetControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            if (String.IsNullOrEmpty(URL.Text))
            {
                throw new NullReferenceException("URL cannot be null");
            }
            if (String.IsNullOrEmpty(Title.Text))
            {
                throw new NullReferenceException("Title of the widget cannot be null");
            }
            if (String.IsNullOrEmpty(Text.Text))
            {
                throw new NullReferenceException("Text cannot be null");
            }
            if (String.IsNullOrEmpty(Attr.Text))
            {
                throw new NullReferenceException("Attribute cannot be null");
            }
            CustomWidget tw = new CustomWidget(URL.Text, Attr.Text, Title.Text, Text.Text);
            await tw.Update();
            return tw;
        }
    }
}
