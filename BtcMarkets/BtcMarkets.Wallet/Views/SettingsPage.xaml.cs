using BtcMarkets.Wallet.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new SettingsViewModel();
        }

        public SettingsViewModel ViewModel { get; private set; }
    }
}