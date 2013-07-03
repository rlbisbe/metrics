using Metrics.Widgets;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Metrics.ViewModel
{
    public class AppViewModel : ViewModelBase
    {
        public List<Service> Services { get; set; }

        public DelegateCommand RefreshCommand { get; private set; }

        public Windows.UI.Xaml.Visibility Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotLoading");
            }
        }

        public bool IsNotLoading
        {
            get
            {
                return Loading == Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        public int UpdatedItems
        {
            get { return updatedItems; }
            set
            {
                updatedItems = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Widget> Items
        {
            get
            {
                if (IsGrouped)
                    return GroupItems();
                
                return IncludeAdControl();
            }
        }

        private ObservableCollection<Widget> IncludeAdControl()
        {
            App myApp = App.Current as App;
            ObservableCollection<Widget> items = myApp.Widgets;
            int index = random.Next(1, items.Count);
            if (items.Contains(adWidget))
                return items;

            items.Insert(index, adWidget);
            return items;
        }

        private App myApp;

        public AppViewModel()
        {
            this.RefreshCommand =
                new DelegateCommand(RefreshCommandExecute,
                    RefreshCommandCanExecute);
            this.myApp = App.Current as App;

            this.Services = new List<Service>();
            Services.Add(new FacebookLikeService());
            Services.Add(new FacebookTalkingAboutService());
            Services.Add(new GithubIssuesService());
            Services.Add(new StackOverflowReputationService());
            Services.Add(new RedditCommentsService());
            Services.Add(new RedditScoreService());

            Services = Services.OrderBy(x => x.MetricsProvider).ToList();
        }

        private bool RefreshCommandCanExecute()
        {
            return true;
        }

        private void RefreshCommandExecute()
        {
            if (!NetworkService.HaveInternetConnection())
                return;

            //UpdateAllAsync();
        }

        private async void UpdateAllAsync()
        {
            Loading = Windows.UI.Xaml.Visibility.Visible;
            UpdatedItems = 0;
            foreach (var item in Items)
            {
                try
                {
                    UpdatedItems += 1;
                    //await item.Update();
                }
                catch (Exception)
                {
                }
            }
            Loading = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public ObservableCollection<Widget> GroupItems()
        {
            App myApp = App.Current as App;
            ObservableCollection<Widget> titles = new ObservableCollection<Widget>();
            if (myApp.Widgets.Contains(adWidget))
            {
                myApp.Widgets.Remove(adWidget);
            }
            foreach (var item in myApp.Widgets)
            {
                if (!titles.Contains(item))
                {
                    titles.Add(new Group(item));
                }
            }
            myApp.Widgets.Remove(adWidget);
            return new ObservableCollection<Widget>(myApp.Widgets.Union(titles).
                OrderBy(x => x.WidgetName).
                ThenBy(x => x.Title).AsQueryable());
        }

        public bool IsGrouped
        {
            get
            {
                return mIsGrouped;
            }
            set
            {
                myApp.IsGrouped = value;
                mIsGrouped = value;
                RaisePropertyChanged("Items");
            }
        }

        public void Add(Widget widget)
        {
            myApp.Widgets.Add(widget);
            CheckForEmptyWidget();
            RaisePropertyChanged("Items");
        }

        private void CheckForEmptyWidget()
        {
            if (myApp.Widgets.Contains(myApp.Empty))
                myApp.Widgets.Remove(myApp.Empty);
        }

        private int updatedItems;
        private Windows.UI.Xaml.Visibility loading;
        private bool mIsGrouped;
        private Random random = new Random();
        private AdWidget adWidget = new AdWidget();

        internal void SwitchAd()
        {
            if (myApp.Widgets.Contains(adWidget))
            {
                myApp.Widgets.Remove(adWidget);
                return;
            }
            IncludeAdControl();
        }

    }
}
