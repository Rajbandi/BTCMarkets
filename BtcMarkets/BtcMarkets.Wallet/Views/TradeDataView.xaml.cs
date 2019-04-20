using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradeDataView : ContentView
    {
        public TradeDataView()
        {
            InitializeComponent();
            DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
            
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            var info = DeviceDisplay.MainDisplayInfo;

            if(info.Orientation == DisplayOrientation.Landscape)
            {

            }
            else
            {
                
            }
        }

        public TradeDataViewModel ViewModel => (TradeDataViewModel)BindingContext;

       
       
    }
}