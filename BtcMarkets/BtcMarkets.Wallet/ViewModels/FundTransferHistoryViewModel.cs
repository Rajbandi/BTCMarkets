using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace BtcMarkets.Wallet.ViewModels
{
    public class FundTransferHistoryViewModel : BaseViewModel
    {
        public FundTransferHistoryViewModel()
        {
            Title = "Fund Transfer History";
            FundHistory = new ObservableCollection<FundTransferData>();
            RefreshData();
        }


        public ObservableCollection<FundTransferData> FundHistory { get; private set; }

        private ObservableCollection<Grouping<string, FundTransferData>> _groupedOrders;
        public ObservableCollection<Grouping<string, FundTransferData>> GroupedOrders
        {
            get => _groupedOrders;
            set => SetProperty(ref _groupedOrders, value, nameof(GroupedOrders));
        }
       
        private bool _hasMore;
        public bool HasMoreFunds
        {
            get => _hasMore;
            set => SetProperty(ref _hasMore, value, nameof(HasMoreFunds));
        }


        public void RefreshData(long? since = null)
        {
            if (!AppData.Current.CheckInternet())
                return;

            if (!AppData.Current.IsAccountSetup)
                return;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Run(async () =>
                {
                    var isLoaderVisible = AppHelper.IsLoaderVisible;

                    if (!isLoaderVisible)
                        IsBusy = true;
                    try
                    {
                        if (!since.HasValue)
                        {
                            FundHistory.Clear();
                        }

                        var appData = AppData.Current;

                        var funds = await appData.GetFundTransferHistory(since);
                        foreach (var fund in funds)
                        {
                            FundHistory.Add(fund);
                        }

                        HasMoreFunds = FundHistory.Count >= 200;
                    }
                    catch (Exception ex)
                    {
                        AppHelper.TrackError(ex);
                        AppHelper.ShowError("Something went wrong.");
                    }

                    if (!isLoaderVisible)
                        IsBusy = false;

                });
            });
        }

        public ICommand RefreshDataCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RefreshData();

                    });

                });
            }
        }
        public ICommand ViewMoreCommand
        {
            get
            {
                return new Command(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                      
                    });
                });
            }
        }
 
    }
}
