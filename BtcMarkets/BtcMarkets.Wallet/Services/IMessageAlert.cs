using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Services
{
    public interface IMessage
    {

        void ShowMessage(string message, bool longAlert = false);
        void ShowMessage(string message, long millingSeconds = 3000);
    }
}
