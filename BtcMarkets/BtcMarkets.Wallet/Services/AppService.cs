using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using BtcMarkets.Wallet.Models;

namespace BtcMarkets.Wallet.Services
{
 
    public class AppService : IAppService
    {
        private IProgressDialog _dialog;

        
        public void ShowAlert(AlertData data)
        {
            throw new NotImplementedException();
        }

        public void ShowLoader(MessageData data = null)
        {
            if(data == null)
            {
                data = new MessageData();
            }


            _dialog = UserDialogs.Instance.Loading(data.Message);
        }

        public void SetLoaderMessage(string message)
        {
            if(_dialog != null)
            {
                _dialog.Title = message;
            }
        }

        public bool IsLoaderShowing => _dialog != null && _dialog.IsShowing;
        public void HideLoader()
        {
            if(_dialog != null && _dialog.IsShowing)
            {
                _dialog.Hide();
            }
        }


        public void ShowToast(ToastData data)
        {
            var toastConfig = new ToastConfig(data.Message);
            
            if(!string.IsNullOrWhiteSpace(data.MessageColor))
            {
                toastConfig.MessageTextColor = Xamarin.Forms.Color.FromHex(data.MessageColor);
            }

            toastConfig.SetDuration(3500);

            if (!string.IsNullOrWhiteSpace(data.BackgroundColor))
            {
                toastConfig.SetBackgroundColor(Xamarin.Forms.Color.FromHex(data.BackgroundColor));
            }

            UserDialogs.Instance.Toast(toastConfig);

        }

        public void ShowError(string error)
        {
            var data = new ToastData
            {
                Message = error,
                BackgroundColor = "#ff0000"
            };

            ShowToast(data);
        }

        public void ShowMessage(string message)
        {
            var data = new ToastData
            {
                Message = message,

            };

            ShowToast(data);
        }

        public void ShowSuccess(string message)
        {
            var data = new ToastData
            {
                Message = message,
                BackgroundColor = "#00ff00"
            };

            ShowToast(data);
        }

        private static AppService _instance;
        public static AppService Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new AppService();
                }
                return _instance;
            }
        }
    }
}
