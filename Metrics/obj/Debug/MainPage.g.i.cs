﻿

#pragma checksum "C:\Users\Roberto\documents\visual studio 11\Projects\Metrics\Metrics\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8AFB2E35E462A3002C232878CE277EA5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Metrics
{
    public partial class MainPage : Metrics.Common.LayoutAwarePage
    {
        private Metrics.Common.LayoutAwarePage pageRoot;
        private Windows.UI.Xaml.Data.CollectionViewSource itemsViewSource;
        private Windows.UI.Xaml.Controls.Grid MainGrid;
        private Windows.UI.Xaml.Controls.ScrollViewer itemGridScrollViewer;
        private Windows.UI.Xaml.Controls.ScrollViewer itemListScrollViewer;
        private Windows.UI.Xaml.Controls.ListView itemListView;
        private Windows.UI.Xaml.Controls.GridView itemGridView;
        private Windows.UI.Xaml.Controls.Button backButton;
        private Windows.UI.Xaml.Controls.TextBlock pageTitle;
        private Windows.UI.Xaml.Controls.Button addButton;
        private Windows.UI.Xaml.Controls.Button refreshButton;
        private Windows.UI.Xaml.Controls.Button deleteButton;
        private Windows.UI.Xaml.VisualState FullScreenLandscape;
        private Windows.UI.Xaml.VisualState Filled;
        private Windows.UI.Xaml.VisualState FullScreenPortrait;
        private Windows.UI.Xaml.VisualState Snapped;
        private bool _contentLoaded;

        [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            Application.LoadComponent(this, new System.Uri("ms-appx:///MainPage.xaml"), Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            pageRoot = (Metrics.Common.LayoutAwarePage)this.FindName("pageRoot");
            itemsViewSource = (Windows.UI.Xaml.Data.CollectionViewSource)this.FindName("itemsViewSource");
            MainGrid = (Windows.UI.Xaml.Controls.Grid)this.FindName("MainGrid");
            itemGridScrollViewer = (Windows.UI.Xaml.Controls.ScrollViewer)this.FindName("itemGridScrollViewer");
            itemListScrollViewer = (Windows.UI.Xaml.Controls.ScrollViewer)this.FindName("itemListScrollViewer");
            itemListView = (Windows.UI.Xaml.Controls.ListView)this.FindName("itemListView");
            itemGridView = (Windows.UI.Xaml.Controls.GridView)this.FindName("itemGridView");
            backButton = (Windows.UI.Xaml.Controls.Button)this.FindName("backButton");
            pageTitle = (Windows.UI.Xaml.Controls.TextBlock)this.FindName("pageTitle");
            addButton = (Windows.UI.Xaml.Controls.Button)this.FindName("addButton");
            refreshButton = (Windows.UI.Xaml.Controls.Button)this.FindName("refreshButton");
            deleteButton = (Windows.UI.Xaml.Controls.Button)this.FindName("deleteButton");
            FullScreenLandscape = (Windows.UI.Xaml.VisualState)this.FindName("FullScreenLandscape");
            Filled = (Windows.UI.Xaml.VisualState)this.FindName("Filled");
            FullScreenPortrait = (Windows.UI.Xaml.VisualState)this.FindName("FullScreenPortrait");
            Snapped = (Windows.UI.Xaml.VisualState)this.FindName("Snapped");
        }
    }
}



