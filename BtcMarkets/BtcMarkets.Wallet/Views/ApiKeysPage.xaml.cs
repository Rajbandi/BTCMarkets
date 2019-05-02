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
    public partial class ApiKeysPage : ContentPage
    {
        public ApiKeysPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new ApiKeysViewModel();
        }

        public ApiKeysViewModel ViewModel { get; private set; }
    }
}