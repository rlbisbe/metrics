using Metrics.Widgets;
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

        public ObservableCollection<Widget> Items
        {
            get
            {
                if (IsGrouped)
                {
                    return GroupItems();
                }
                App myApp = App.Current as App;
                return myApp.Widgets;
            }
        }

        private ObservableCollection<Widget> groupedItems;

        public AppViewModel()
        {
            this.RefreshCommand =
                new DelegateCommand(RefreshCommandExecute,
                    RefreshCommandCanExecute);
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
                    await item.Update();
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
            foreach (var item in myApp.Widgets)
            {
                if (!titles.Contains(item))
                {
                    titles.Add(new Group(item));
                }
            }
            return new ObservableCollection<Widget>(myApp.Widgets.Union(titles).OrderBy(x => x.WidgetName).ThenBy(x => x.Title).AsQueryable());
        }

        public bool IsGrouped
        {
            get
            {
                return mIsGrouped;
            }
            set
            {
                App myApp = App.Current as App;
                myApp.IsGrouped = value;
                mIsGrouped = value;
                RaisePropertyChanged("Items");
            }
        }

        private int updatedItems;
        private Windows.UI.Xaml.Visibility loading;
        private bool mIsGrouped;
    }
}
