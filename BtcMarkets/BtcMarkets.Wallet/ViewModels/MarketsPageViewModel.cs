using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using BtcMarkets.Wallet.Helpers;

namespace BtcMarkets.Wallet.ViewModels
{

    public class MarketsPageViewModel : BaseViewModel
    {
        private ObservableCollection<Grouping<GroupKey, Market>> _groupedMarkets;

        public ObservableCollection<Grouping<GroupKey, Market>> GroupedMarkets
        {
            get => _groupedMarkets;
            set => SetProperty(ref _groupedMarkets, value, nameof(GroupedMarkets));
        }

        private bool _isSearchBarVisible;
        public bool IsSearchBarVisible
        {
            get => _isSearchBarVisible;
            set => SetProperty(ref _isSearchBarVisible, value);
        }
        
        public MarketsPageViewModel()
        {
            Subscribe();
            RefreshMarkets();
          
        }
        public void RefreshMarkets()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
               // await Task.Delay(100);
                var appData = AppData.Current;
                if (appData.Markets == null || !appData.Markets.Any())
                {
                    await appData.InitMarkets();

               
                }
                else
                {
                    await appData.RefreshMarkets();
                }
                
                RefreshGroups();
               
              
            });
        }
        
        public Market SelectedMarket { get; set; }

        Grouping<GroupKey, Market> _favGroups, _audGroups, _btcGroups;

        GroupKey _favKey, _audKey, _btcKey;

        List<Market> _favourites, _audMarkets, _btcMarkets;
        public void RefreshGroups()
        {
            Device.BeginInvokeOnMainThread( async () =>
            {
                var markets = AppData.Current.Markets;

                if (_favKey == null)
                {
                    _favKey = new GroupKey("FAV", "Favourites");
                }
                if (_audKey == null)
                {
                    _audKey = new GroupKey("AUD", "AUD Markets");
                }
                if (_btcKey == null)
                {
                    _btcKey = new GroupKey("BTC", "BTC Markets");
                }

                _favourites = new List<Market>(markets.Where(x => x.Starred));
                _audMarkets = new List<Market>(markets.Where(x => x.Currency == Constants.Aud));
                _btcMarkets = new List<Market>(markets.Where(x => x.Currency == Constants.Btc));

                var favourites = new List<Market>();
                if (_favKey.IsExpanded)
                {
                    favourites.AddRange(_favourites);
                }

                var audMarkets = new List<Market>();
                if (_audKey.IsExpanded)
                {
                    audMarkets.AddRange(_audMarkets);
                }

                var btcMarkets = new List<Market>();
                if (_btcKey.IsExpanded)
                {
                    btcMarkets.AddRange(_btcMarkets);
                }

                _favKey.HelpText = $" ({_favourites.Count()} Markets)";
                _audKey.HelpText = $" ({_audMarkets.Count()} Markets)";
                _btcKey.HelpText = $" ({_btcMarkets.Count()} Markets)";

                _favGroups = new Grouping<GroupKey, Market>(_favKey, favourites);
                _audGroups = new Grouping<GroupKey, Market>(_audKey, audMarkets);
                _btcGroups = new Grouping<GroupKey, Market>(_btcKey, btcMarkets);
                GroupedMarkets = new ObservableCollection<Grouping<GroupKey, Market>>
                                {
                                    _favGroups,
                                    _audGroups,
                                    _btcGroups
                                };

                await Task.Delay(400);
               
                IsBusy = false;
            });
        }

        private void Current_MarketUpdated(object sender, MarketEventArgs e)
        {
            var market = e.Market;


            Market updateMarket = _favGroups.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);

            if (updateMarket == null)
            {
                updateMarket = _audGroups.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);

                if (updateMarket == null)
                {
                    updateMarket = _btcGroups.FirstOrDefault(x => x.Instrument == market.Instrument && x.Currency == market.Currency);
                }
            }

            if (updateMarket != null)
            {
                updateMarket.Ask = market.Ask;
                updateMarket.Bid = market.Bid;
                updateMarket.LastPrice = market.LastPrice;

                if(updateMarket.Volume != market.Volume && market.Volume>0)
                    updateMarket.Volume = market.Volume;

                updateMarket.Change = market.Change;
                updateMarket.Holdings = market.Holdings;
              

            }
        }

        public void Subscribe()
        {
            AppData.Current.MarketUpdated += Current_MarketUpdated;
        }

        public void Unsubscribe()
        {
            AppData.Current.MarketUpdated -= Current_MarketUpdated;
        }

        public ICommand ExpandGroupCommand
        {
            get
            {
                return new Command((arg) =>
                {
                    var grp = (GroupKey)arg;

                    if (grp != null)
                    {
                        grp.IsExpanded = !grp.IsExpanded;
                        bool refresh = true;
                        if (grp.Code == _favKey.Code)
                        {
                            _favKey.IsExpanded = grp.IsExpanded;
                        }
                        else
                            if (grp.Code == _audKey.Code)
                        {
                            _audKey.IsExpanded = grp.IsExpanded;
                        }
                        else
                            if (grp.Code == _btcKey.Code)
                        {
                            _btcKey.IsExpanded = grp.IsExpanded;
                        }
                        else
                        {
                            refresh = false;
                        }
                        if (refresh)
                            RefreshGroups();
                    }
                });
            }
        }

        public ICommand FavouriteCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var market = (Market)item;
                    if (market != null)
                    {
                        var state = market.Starred;
                        market.Starred = !state;

                        var data = AppData.Current;

                        data.AddOrRemoveFavourite(market, state);
                        RefreshGroups();
                    }
                });
            }
        }
        public ICommand NotificationCommand
        {
            get
            {
                return new Command((item) =>
                {
                    var market = (Market)item;
                    if (market != null)
                    {
                        market.Notification = !market.Notification;
                    }

                });
            }
        }

        public ICommand ShowSearchCommand
        {
            get
            {
                return new Command((arg) =>
                {
                    IsSearchBarVisible = !IsSearchBarVisible;
                });
            }
        }

        public ICommand RefreshDataCommand
        {
            get
            {

                return new Command(() =>
                {
                    // Refresh();
                    RefreshMarkets();
                });
            }
        }
    }
}


