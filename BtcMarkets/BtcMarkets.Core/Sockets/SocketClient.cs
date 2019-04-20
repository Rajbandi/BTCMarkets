using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BtcMarkets.Core.Api;
using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;

namespace BtcMarkets.Core.Sockets
{
    public class SocketClient
    {
        private Socket _socket;
        private List<SocketMarket> _markets;

        public const string TickerEvent = "newTicker";
        public const string OrderBookEvent = "OrderBookChange";
        public const string MarketTradeEvent = "MarketTrade";

        public const string TickerChannel = "Ticker-BTCMarkets-";
        public const string OrderChannel = "Orderbook_";
        public const string MarketTradeChannel = "TRADE_";

        public event EventHandler<TickerEventArgs> TickerChanged;
        public event EventHandler<OrderBookEventArgs> OrderBookChanged;
        public event EventHandler<MarketTradeEventArgs> MarketTradeChanged;

        public event EventHandler<SocketEventArgs> Connected;
        public event EventHandler<SocketEventArgs> Disconnected;

        public List<SocketMarket> Markets => _markets;

        public bool IsOpen { get; private set; }

        public SocketClient()
        {
            var transport = "websocket";
            var url = "https://socket.btcmarkets.net";
            var options = new IO.Options
            {
                Secure = true,
                Transports = ImmutableList.Create<string>(transport),
                Upgrade = false,
                AutoConnect = false,

            };

            _socket = IO.Socket(url, options);

            _socket.On("connect", () =>
            {
                IsOpen = true;
                if (_connectActions != null && _connectActions.Any())
                {
                    foreach (var action in _connectActions)
                    {
                        try
                        {
                            action.Invoke();
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    _connectActions.Clear();
                }
                Connected?.Invoke(this, new SocketEventArgs());
            });

            _socket.On("disconnect", () =>
            {
                IsOpen = false;
                Disconnected?.Invoke(this, new SocketEventArgs());
            });

            _socket.On(TickerEvent, (data) =>
            {
                var tickerArgs = new TickerEventArgs();
                var jsonData = (data ?? "").ToString();
                tickerArgs.Data = jsonData;
                try
                {
                    var ticker = JsonConvert.DeserializeObject<TickerData>(jsonData);
                    tickerArgs.Ticker = ticker;
                }
                catch (Exception ex)
                {
                    tickerArgs.Error = $"{ex.Message} {ex.StackTrace}";
                }
                TickerChanged?.Invoke(this, tickerArgs);
            });

            _socket.On(OrderBookEvent, (data) =>
            {
                var orderArgs = new OrderBookEventArgs();
                var jsonData = (data ?? "").ToString();
                orderArgs.Data = jsonData;
                try
                {
                    orderArgs.OrderBook = JsonConvert.DeserializeObject<OrderBookData>(jsonData);
                }
                catch (Exception ex)
                {
                    orderArgs.Error = $"{ex.Message} {ex.StackTrace}";
                }
                OrderBookChanged?.Invoke(this, orderArgs);
            });

            _socket.On(MarketTradeEvent, (data) =>
            {
                var marketArgs = new MarketTradeEventArgs();
                var jsonData = (data ?? "").ToString();
                marketArgs.Data = jsonData;
                try
                {
                    marketArgs.MarketTrade = JsonConvert.DeserializeObject<MarketTradeData>(jsonData);
                }
                catch (Exception ex)
                {
                    marketArgs.Error = $"{ex.Message} {ex.StackTrace}";
                }
                MarketTradeChanged?.Invoke(this, marketArgs);
            });
        }

        private List<Action> _connectActions = new List<Action>();

        public void Open(Action action=null)
        {
            if (action != null)
            {
                var connectAction = _connectActions.FirstOrDefault(x => x == action);
                if (connectAction == null)
                {
                    _connectActions.Add(action);
                }
            }
            _socket?.Open();
        }

        
        public void Close()
        {
            _socket?.Close();
        }
        
     
        public void JoinChannels(List<SocketMarket> markets = null)
        {
            if (!IsOpen)
                return;
            _markets = markets ?? new List<SocketMarket>
            {
                new SocketMarket
                {
                    Instrument = ApiConstants.Btc,
                    Currency = ApiConstants.Aud,
                    Ticker = true,
                    OrderBook = true,
                    Trade = true
                }
            };

            var join = "join";
            foreach (var market in _markets)
            {
                var marketPair = GetMarketPair(market);
                if (market.Ticker)
                {
                    _socket.Emit(join, TickerChannel + GetMarketPair(market, "-"));
                }

                if (market.OrderBook)
                {
                    _socket.Emit(join, OrderChannel + marketPair);
                }

                if (market.Trade)
                {
                    _socket.Emit(join, MarketTradeChannel + marketPair);
                }
            }

        }

        public void LeaveChannels(List<SocketMarket> markets = null)
        {
            if (!IsOpen)
                return;

            var channelMarkets = markets ?? _markets;
            if (channelMarkets != null)
            {
                var leave = "leave";
                foreach (var market in channelMarkets)
                {
                    var marketPair = GetMarketPair(market);
                    if (market.Ticker)
                    {
                        _socket.Emit(leave, TickerChannel + GetMarketPair(market, "-"));
                    }

                    if (market.OrderBook)
                    {
                        _socket.Emit(leave, OrderChannel + marketPair);
                    }

                    if (market.Trade)
                    {
                        _socket.Emit(leave, MarketTradeChannel + marketPair);
                    }
                }
            }
        }

        private string GetMarketPair(SocketMarket market, string sep = "")
        {
            return $"{market.Instrument}{sep}{market.Currency}";
        }

    }
}
