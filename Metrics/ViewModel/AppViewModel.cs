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
        public DelegateCommand RefreshCommand { get; private set; }

        public Windows.UI.Xaml.Visibility Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                RaisePropertyChanged();
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

        public ObservableCollection<Object> Items
        {
            get
            {
                if (IsGrouped)
                    return GroupItems();
                
                return IncludeAdControl();
            }
        }

        private ObservableCollection<object> IncludeAdControl()
        {
            App myApp = App.Current as App;
            ObservableCollection<Object> items = myApp.Widgets;
            int index = random.Next(1, items.Count);
            if (items.Contains(adControl))
                items.Remove(adControl);

            items.Insert(index, adControl);
            return items;
        }

        private App myApp;

        public AppViewModel()
        {
            this.RefreshCommand =
                new DelegateCommand(RefreshCommandExecute,
                    RefreshCommandCanExecute);
            this.myApp = App.Current as App;
        }

        private bool RefreshCommandCanExecute()
        {
            return true;
        }

        private void RefreshCommandExecute()
        {
            if (!NetworkService.HaveInternetConnection())
                return;

            UpdateAllAsync();
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
                    if (item as Widget == null)
                        continue;

                    await (item as Widget).Update();
                }
                catch (Exception)
                {
                }
            }
            Loading = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public ObservableCollection<Object> GroupItems()
        {
            App myApp = App.Current as App;
            ObservableCollection<Object> titles = new ObservableCollection<Object>();
            foreach (var item in myApp.Widgets)
            {
                if (!titles.Contains(item))
                {
                    titles.Add(new Group(item as Widget));
                }
            }
            myApp.Widgets.Remove(adControl);
            return new ObservableCollection<Object>(myApp.Widgets.Union(titles).
                OrderBy(x => (x as Widget).WidgetName).
                ThenBy(x => (x as Widget).Title).AsQueryable());
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
        private AdControl adControl = new AdControl();
    }
}
