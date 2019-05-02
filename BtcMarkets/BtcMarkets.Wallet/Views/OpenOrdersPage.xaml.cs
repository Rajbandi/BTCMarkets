using BtcMarkets.Wallet.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpenOrdersPage : ContentPage
    {
        public OpenOrdersPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new OpenOrdersViewModel();
        }

        public OpenOrdersViewModel ViewModel { get; private set; }

       
    }
}