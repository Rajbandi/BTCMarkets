using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class AccountBalancesViewModel : BaseViewModel
    {
        public AccountBalancesViewModel()
        {
         

        }


        public ICommand RefreshDataCommand
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }
    }
}
