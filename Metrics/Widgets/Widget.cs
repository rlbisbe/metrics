using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metrics.Widgets;
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

        private int counter;

        public int Counter
        {
            get { return counter; }            
            set 
            { 
                counter = value;
                NotifyPropertyChanged("Counter");
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
            string type = (string)val["name"];

            switch (type)
            {
                case "TweetWidget":
                    return new TweetWidget((string)val["source"]);
                case "FacebookWidget":
                    return new FacebookWidget((string)val["source"]);
                case "TuentiWidget":
                    return new TuentiWidget((string)val["source"]);
                case "GithubWidget":
                    return new GithubWidget((string)val["username"], (string)val["repository"]);
                case "StackOverflowWidget":
                    return new StackOverflowWidget((string)val["source"]);
                case "WordpressWidget":
                    return new WordpressWidget((string)val["blog"], (string)val["key"]);
                default:
                    return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
