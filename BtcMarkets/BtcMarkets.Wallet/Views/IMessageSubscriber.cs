using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Wallet.Views
{
    public interface IMessageSubscriber
    {

        void Subscribe();

        void Unsubscribe();
    }
}
