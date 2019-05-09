using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";
            LoadDetails();
        }

        public string SectionColor
        {
            get
            {
                var style = (Color)Application.Current.Resources["AccentColor"];
                return style.ToHexWeb();
            }
        }
        public string SwitchColor
        {
            get
            {
                var style = (Color)Application.Current.Resources["PrimaryColor"];
                return style.ToHexWeb();
            }
        }
        private bool _liveUpdates;
        public bool LiveUpdates
        {
            get => _liveUpdates;
            set => SetProperty(ref _liveUpdates, value, nameof(LiveUpdates));
        }

        private string _theme;
        public string Theme
        {
            get => _theme;
            set => SetProperty(ref _theme, value, nameof(Theme));
        }

        private string _apiKey;
        public string ApiKey
        {
            get => _apiKey;
            set => SetProperty(ref _apiKey, value, nameof(ApiKey));
        }

        private string _secret;
        public string Secret
        {
            get => _secret;
            set => SetProperty(ref _secret, value, nameof(Secret));
        }

        public void LoadDetails()
        {
            
            var data = AppData.Current;
            var settings = data.Settings;

            LiveUpdates = settings.LiveUpdates;
            var theme = settings.Theme;
            if(string.IsNullOrWhiteSpace(theme))
            {
                theme = "Dark";
            }
            Theme = theme;

            var credentials = settings.ApiCredentials;
            if (credentials != null)
            {
                ApiKey = credentials.ApiKey ?? "";
                Secret = credentials.Secret ?? "";
            }
            

        }
        public async void SaveDetails()
        {
            IsBusy = true;
            var data = AppData.Current;
            var settings = data.Settings;
            var isValid = await VerifyApiData();
            if(!isValid)
            {
                AppHelper.ShowError("Invalid api credentials.");
            }

            settings.LiveUpdates = LiveUpdates;
            settings.Theme = Theme;

            data.SaveSettings(true);
            IsBusy = false;
        }

        public async Task<bool> VerifyApiData()
        {
            var isValid = true;
            var data = AppData.Current;
            var settings = data.Settings;
            if(settings != null)
            {
                var credentials = settings.ApiCredentials;

                if(!string.IsNullOrEmpty(ApiKey) && !string.IsNullOrEmpty(Secret) 
                    && ApiKey != credentials.ApiKey && Secret != credentials.Secret)
                {
                    var result = await data.Client.CheckApiCredentials(ApiKey, Secret);
                    if(result)
                    {
                        credentials.ApiKey = ApiKey;
                        credentials.Secret = Secret;
                    }
                    return result;
                }
            }
            return isValid;
        }

        public ICommand SaveCommand
        {
            get
            {
                return new Command(() =>
                {
                    SaveDetails();        

                });
            }
        }

    }
}
