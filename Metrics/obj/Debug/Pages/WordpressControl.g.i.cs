﻿

#pragma checksum "C:\Users\Roberto\Documents\Visual Studio 11\Projects\Metrics\Metrics\Pages\WordpressControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "64F53A76D49491B1687CF41EF43C9F67"
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
    public partial class WordpressControl : Windows.UI.Xaml.Controls.UserControl
    {
        private Windows.UI.Xaml.Controls.ComboBox Metric;
        private Windows.UI.Xaml.Controls.TextBox URL;
        private Windows.UI.Xaml.Controls.TextBox API;
        private bool _contentLoaded;

        [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            Application.LoadComponent(this, new System.Uri("ms-appx:///Pages/WordpressControl.xaml"), Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            Metric = (Windows.UI.Xaml.Controls.ComboBox)this.FindName("Metric");
            URL = (Windows.UI.Xaml.Controls.TextBox)this.FindName("URL");
            API = (Windows.UI.Xaml.Controls.TextBox)this.FindName("API");
        }
    }
}



