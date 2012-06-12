using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metrics.Widgets;
using Windows.Globalization.NumberFormatting;
using Windows.Storage;

namespace Metrics
{
    public abstract class Widget : INotifyPropertyChanged
    {
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string widgetName;

        public string WidgetName
        {
            get { return widgetName; }
            set
            {
                widgetName = value;
                NotifyPropertyChanged("WidgetName");
            }
        }

        private int counter;

        public int Counter
        {
            get { return counter; }
            set
            {
                counter = value;
                DecimalFormatter decimalFormat1 = new Windows.Globalization.NumberFormatting.DecimalFormatter();
                decimalFormat1.IsGrouped = true;
                decimalFormat1.FractionDigits = 0;
                SCounter = decimalFormat1.Format(counter);
                NotifyPropertyChanged("Counter");
            }
        }

        private string scounter;
        public string SCounter
        {
            get { return scounter; }
            set
            {
                scounter = value;
                NotifyPropertyChanged("SCounter");
            }
        }

        private string background;

        public string Background
        {
            get { return background; }
            set
            {
                background = value;
                NotifyPropertyChanged("Background");
            }
        }

        private string foreground;

        public string Foreground
        {
            get { return foreground; }
            set
            {
                foreground = value;
                NotifyPropertyChanged("Foreground");
            }
        }

        private string widgetforeground;

        public string WidgetForeground
        {
            get { return widgetforeground; }
            set
            {
                widgetforeground = value;
                NotifyPropertyChanged("WidgetForeground");
            }
        }

        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        /// <summary>
        /// Updates the content of the widget, used also for initialization.
        /// </summary>
        /// <returns></returns>
        public abstract Task Update();

        public abstract ApplicationDataCompositeValue Save();

        public static Widget CreateWidget(ApplicationDataCompositeValue val)
        {
            if (val == null)
            {
                return null;
            }
            string type = (string)val["name"];

            switch (type)
            {
                case "TweetWidget":
                    return new TweetWidget((string)val["source"]);
                case "FacebookWidget":
                    return new FacebookWidget((string)val["source"], (string)val["selection"]);
                case "TuentiWidget":
                    return new TuentiWidget((string)val["source"]);
                case "GithubWidget":
                    return new GithubWidget((string)val["username"], (string)val["repository"]);
                case "StackOverflowWidget":
                    return new StackOverflowWidget((string)val["source"], (string)val["site"]);
                case "WordpressWidget":
                    return new WordpressWidget((string)val["blog"], (string)val["key"], (string)val["selection"]);
                case "CustomWidget":
                    return new CustomWidget((string)val["url"], (string)val["data"], (string)val["widgetName"], (string)val["title"]);
                
                default:
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
