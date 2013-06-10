using Metrics.Widgets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Metrics.ViewModel
{
    public class Value : INotifyPropertyChanged
    {
        public string Content 
        {
            get
            {
                return mContent;
            }
            set {
                mContent = value;
                RaisePropertyChanged();
            }
        }

        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string mContent;
    }

    public abstract class Service
    {
        public string MetricsName { get; set; }
        public string MetricsProvider { get; set; }
        public Dictionary<string, Value> Properties { get; set; }
        private string Url { get; set; }

        public Service()
        {
            Properties = new Dictionary<string, Value>();
        }

        public abstract Task<Widget> GetWidget();
    }
}
