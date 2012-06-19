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
            ErrorUrl.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ErrorTitle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ErrorText.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ErrorAttribute.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            bool error = false;
            if (String.IsNullOrEmpty(URL.Text))
            {
                ErrorUrl.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (String.IsNullOrEmpty(Title.Text))
            {
                ErrorTitle.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (String.IsNullOrEmpty(Text.Text))
            {
                ErrorText.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (String.IsNullOrEmpty(Attr.Text))
            {
                ErrorAttribute.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (error == true)
            {
                return null;
            }
            CustomWidget tw = new CustomWidget(URL.Text, Attr.Text, Title.Text, Text.Text);
            await tw.Update();
            return tw;
        }
    }
}
