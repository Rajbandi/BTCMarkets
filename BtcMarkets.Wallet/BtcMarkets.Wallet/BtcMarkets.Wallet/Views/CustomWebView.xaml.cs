using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomWebView : ContentView
    {
        public CustomWebView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty HtmlSourceProperty = BindableProperty.Create(nameof(HtmlSource), typeof(string), typeof(CustomWebView), default(string), BindingMode.OneWay);
        public string HtmlSource
        {
            get
            {
                return (string)GetValue(HtmlSourceProperty);
            }
            set
            {
                SetValue(HtmlSourceProperty, value);
            }
        }

        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(CustomWebView), default(bool), BindingMode.OneWay);
        public bool IsLoading
        {
            get
            {
                return (bool)GetValue(IsLoadingProperty);
            }
            set
            {
                SetValue(IsLoadingProperty, value);
            }
        }

        public static readonly BindableProperty CustomHeightProperty = BindableProperty.Create(nameof(Height), typeof(double), typeof(CustomWebView), default(double), BindingMode.OneWay);
        public double CustomHeight
        {
            get
            {
                return (double)GetValue(CustomHeightProperty);
            }
            set
            {
                SetValue(CustomHeightProperty, value);
            }
        }

        public static readonly BindableProperty CustomWidthProperty = BindableProperty.Create(nameof(Width), typeof(double), typeof(CustomWebView), default(double), BindingMode.OneWay);
        public double CustomWidth
        {
            get
            {
                return (double)GetValue(CustomWidthProperty);
            }
            set
            {
                SetValue(CustomWidthProperty, value);
            }
        }
    }
}