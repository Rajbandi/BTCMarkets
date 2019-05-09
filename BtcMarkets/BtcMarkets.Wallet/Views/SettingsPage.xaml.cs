using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private ZXingScannerPage _scanner;
        private int _mode;
        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new SettingsViewModel();
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                TryInverted = true,
                PossibleFormats = new List<BarcodeFormat>
                                    {
                                       BarcodeFormat.QR_CODE, BarcodeFormat.EAN_13,BarcodeFormat.DATA_MATRIX
                                    }
            };
            _scanner = new ZXingScannerPage(options)
            {
                DefaultOverlayTopText = "Align the barcode within the frame",
                DefaultOverlayBottomText = string.Empty,
                DefaultOverlayShowFlashButton = true
            };
            _scanner.OnScanResult += Scanner_OnScanResult;
        }

        public SettingsViewModel ViewModel { get; private set; }

        private void ApiKeyQr_Tapped(object sender, System.EventArgs e)
        {
            CaptureApiKey();
        }
        private void SecretQr_Tapped(object sender, System.EventArgs e)
        {
            CaptureSecret();
        }

        private async void CaptureApiKey()
        {
            _mode = 1;
            await Navigation.PushAsync(_scanner);
        }
        private async void CaptureSecret()
        {
            _mode = 2;
            await Navigation.PushAsync(_scanner);
        }

        private void Scanner_OnScanResult(ZXing.Result result)
        {
            try
            {
                _scanner.IsAnalyzing = false;
                _scanner.IsScanning = false;

                if (result != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            await Navigation.PopAsync();
                            if (_mode == 1)
                            {
                                ViewModel.ApiKey = result.Text;
                            }
                            else
                            if (_mode == 2)
                            {
                                ViewModel.Secret = result.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            AppHelper.TrackError(ex);
                            AppHelper.ShowError("Something went wrong. Try again.");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                AppHelper.TrackError(ex);
                AppHelper.ShowError("Something went wrong. Try again.");
            }
        }

      
    }
}