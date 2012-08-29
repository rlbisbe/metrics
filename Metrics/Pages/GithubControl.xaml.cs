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
    public sealed partial class GithubControl : UserControl, IWidget
    {
        public GithubControl()
        {
            this.InitializeComponent();
        }

        async public Task<Widget> GetWidget()
        {
            UserError.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            RepositoryError.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bool error = false;
            if (String.IsNullOrEmpty(Username.Text))
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                UserError.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (String.IsNullOrEmpty(Repository.Text))
            {
                var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
                RepositoryError.Visibility = Windows.UI.Xaml.Visibility.Visible;
                error = true;
            }
            if (error == true)
            {
                return null;
            }
            if ((Metric.SelectedItem as ComboBoxItem).Content.Equals(OpenIssues.Content))
            {
                GithubWidget tw = new GithubWidget(Username.Text,Repository.Text);
                await tw.Update();
                return tw;
            }
            return null;
        }
    }
}
