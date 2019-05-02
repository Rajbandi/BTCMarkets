using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Services
{
    
    public interface IAppService
    {
        void ShowLoader(MessageData data = null);

        void HideLoader();

        void ShowAlert(AlertData data);


        void ShowToast(ToastData data);

        void ShowError(string error);

        void ShowMessage(string message);

        void ShowSuccess(string message);

        void SetLoaderMessage(string message);

        
    }
}
