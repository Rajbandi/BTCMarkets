using BtcMarkets.Wallet.ViewModels;
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
    public partial class NewsPage : ContentPage
    {
        public NewsPageViewModel ViewModel { get; private set; }
        public NewsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new NewsPageViewModel();
        }

      
    }
}