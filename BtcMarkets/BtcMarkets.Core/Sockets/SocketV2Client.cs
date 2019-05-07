using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using BtcMarkets.Core.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureWebSockets;

namespace BtcMarkets.Core.Sockets
{

    public static class MessageType
    {
        public const string Tick = "tick";
        public const string HeartBeat = "heartbeat";
        public const string OrderBook = "orderbook";
        public const string OrderChange = "orderChange";
        public const string FundChange = "fundChange";
        public const string Trade = "trade";
        public const string Error = "error";
    }

    public static class ChannelType
    {
        public const string Tick = "tick";
        public const string HeartBeat = "heartbeat";
        public const string OrderBook = "orderbook";
        public const string OrderChange = "orderChange";
        public const string FundChange = "fundChange";
        public const string Trade = "trade";

    }

    public enum SocketState
    {
        None,
        Open,
        Close,
        Opening,
        Closing,
        Abort

    }
    public class SocketV2Client
    {
        private EventHandler<TickerDataV2> _tickerChanged;
        public event EventHandler<TickerDataV2> TickerChanged
        {
            add
            {
                if (_tickerChanged == null || !_tickerChanged.GetInvocationList().Contains(value))
                {
                    _tickerChanged += value;
                }
            }
            remove
            {
                if (_tickerChanged != null && _tickerChanged.GetInvocationList().Contains(value))
                    _tickerChanged -= value;
            }
        }

        private EventHandler<OrderBookDataV2> _orderBookChanged;
        public event EventHandler<OrderBookDataV2> OrderBookChanged
        {
            add
            {
                if (_orderBookChanged == null || !_orderBookChanged.GetInvocationList().Contains(value))
                {
                    _orderBookChanged += value;
                }
            }
            remove
            {
                if (_orderBookChanged != null && _orderBookChanged.GetInvocationList().Contains(value))
                    _orderBookChanged -= value;
            }
        }


        private EventHandler<OrderChangeDataV2> _orderChanged;
        public event EventHandler<OrderChangeDataV2> OrderChanged
        {
            add
            {
                if (_orderChanged == null || !_orderChanged.GetInvocationList().Contains(value))
                {
                    _orderChanged += value;
                }
            }
            remove
            {
                if (_orderChanged != null && _orderChanged.GetInvocationList().Contains(value))
                    _orderChanged -= value;
            }
        }

        private EventHandler<FundTransferDataV2> _fundTransferred;
        public event EventHandler<FundTransferDataV2> FundTransferred
        {
            add
            {
                if (_fundTransferred == null || !_fundTransferred.GetInvocationList().Contains(value))
                {
                    _fundTransferred += value;
                }
            }
            remove
            {
                if (_fundTransferred != null && _fundTransferred.GetInvocationList().Contains(value))
                    _fundTransferred -= value;
            }
        }


        private EventHandler<MarketTradeDataV2> _marketTradeChanged;
        public event EventHandler<MarketTradeDataV2> MarketTradeChanged
        {
            add
            {
                if (_marketTradeChanged == null || !_marketTradeChanged.GetInvocationList().Contains(value))
                {
                    _marketTradeChanged += value;
                }
            }
            remove
            {
                if (_marketTradeChanged != null && _marketTradeChanged.GetInvocationList().Contains(value))
                    _marketTradeChanged -= value;
            }
        }

        private EventHandler<ErrorV2> _messageErrorReceived;
        public event EventHandler<ErrorV2> ErrorMessageReceived
        {
            add
            {
                if (_messageErrorReceived == null || !_messageErrorReceived.GetInvocationList().Contains(value))
                {
                    _messageErrorReceived += value;
                }
            }
            remove
            {
                if (_messageErrorReceived != null && _messageErrorReceived.GetInvocationList().Contains(value))
                    _messageErrorReceived -= value;
            }
        }

        private EventHandler<SocketState> _stateChanged;
        public event EventHandler<SocketState> StateChanged
        {
            add
            {
                if (_stateChanged == null || !_stateChanged.GetInvocationList().Contains(value))
                {
                    _stateChanged += value;
                }
            }
            remove
            {
                if (_stateChanged != null && _stateChanged.GetInvocationList().Contains(value))
                    _stateChanged -= value;
            }
        }

        private EventHandler _opened;
        public event EventHandler Opened
        {
            add
            {
                if (_opened == null || !_opened.GetInvocationList().Contains(value))
                {
                    _opened += value;
                }
            }
            remove
            {
                if (_opened != null && _opened.GetInvocationList().Contains(value))
                    _opened -= value;
            }
        }

        private EventHandler _closed;
        public event EventHandler Closed
        {
            add
            {
                if (_closed == null || !_closed.GetInvocationList().Contains(value))
                {
                    _closed += value;
                }
            }
            remove
            {
                if (_closed != null && _closed.GetInvocationList().Contains(value))
                    _closed -= value;
            }
        }

        private EventHandler _sendFailed;
        public event EventHandler SendFailed
        {
            add
            {
                if (_sendFailed == null || !_sendFailed.GetInvocationList().Contains(value))
                {
                    _sendFailed += value;
                }
            }
            remove
            {
                if (_sendFailed != null && _sendFailed.GetInvocationList().Contains(value))
                    _sendFailed -= value;
            }
        }

        private EventHandler<Exception> _onError;
        public event EventHandler<Exception> OnError
        {
            add
            {
                if (_onError == null || !_onError.GetInvocationList().Contains(value))
                {
                    _onError += value;
                }
            }
            remove
            {
                if (_onError != null && _onError.GetInvocationList().Contains(value))
                    _onError -= value;
            }
        }

        public SocketState State { get; private set; }


        private PureWebSocket _socket;
        private PureWebSocketOptions _options;
        private SocketOptions options;
        public bool IsOpen => _socket.State == WebSocketState.Open;
        public SocketV2Client(SocketOptions opt)
        {
            options = opt;
            _options = new PureWebSocketOptions
            {
                IgnoreCertErrors = true,
                MyReconnectStrategy = new ReconnectStrategy(2000, 4000, 20)
            };

            _socket = new PureWebSocket(options.Url, _options);

            _socket.OnStateChanged += _socket_OnStateChanged;
            _socket.OnMessage += _socket_OnMessage;
            _socket.OnClosed += _socket_OnClosed;
            _socket.OnSendFailed += _socket_OnSendFailed;
            _socket.OnOpened += _socket_OnOpened;
            _socket.OnError += _socket_OnError;

        }

        public async Task Open()
        {
            try
            {
                await _socket.ConnectAsync();
            }
            catch (Exception ex)
            {
                _onError.Invoke(this, ex);
            }
        }
        public void Close()
        {
            _socket.Disconnect();
        }

        public async Task Subscribe(SubscribeMessage message)
        {
            try
            {
                SignSubcribeMessage(ref message);
                var json = JsonConvert.SerializeObject(message);
                await _socket.SendAsync(json);
                
            }
            catch(Exception ex)
            {
                _onError.Invoke(this, ex);
            }
        }

        private void SignSubcribeMessage(ref SubscribeMessage message)
        {
            var buffer = new StringBuilder();
            var timestamp = ApiHelper.GetTimeStamp();
            var action = "/users/self/subscribe";

            buffer.Append(action + "\n");
            buffer.Append(timestamp );

         
            var data = buffer.ToString();
            if (!string.IsNullOrWhiteSpace(options.Key))
            {
                if (!string.IsNullOrWhiteSpace(data) && !string.IsNullOrWhiteSpace(options.Secret))
                {
                    message.Signature = ApiHelper.SignData(data, options.Secret);

                    message.Key = options.Key;
                    
                }
            }

            message.Timestamp = timestamp;
        }
        public void HeartBeat(HeartBeatMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            Console.WriteLine(json);
            _socket.Send(json);
        }
        public void UnSubscribe(SubscribeMessage message)
        {

        }

        private void _socket_OnOpened()
        {
            _opened?.Invoke(this, EventArgs.Empty);
        }

        private void _socket_OnClosed(WebSocketCloseStatus reason)
        {
            _closed?.Invoke(this, EventArgs.Empty);
        }

        private void _socket_OnStateChanged(WebSocketState newState, WebSocketState prevState)
        {
            SocketState state = SocketState.None;
            switch (newState)
            {
                case WebSocketState.Open:
                    state = SocketState.Open;
                    break;
                case WebSocketState.Closed:
                    state = SocketState.Close;
                    break;
                case WebSocketState.Connecting:
                    state = SocketState.Opening;
                    break;
                case WebSocketState.Aborted:
                    state = SocketState.Abort;
                    break;
                case WebSocketState.CloseSent:
                    state = SocketState.Closing;
                    break;
            }
            _stateChanged?.Invoke(this, state);
            this.State = state;

        }

        private void _socket_OnMessage(string message)
        {
            try
            {
                var json = JValue.Parse(message);
                var messageType = (string)json["messageType"];
                switch (messageType)
                {
                    case MessageType.Error:

                        var error = json.ToObject<ErrorV2>();
                        _messageErrorReceived?.Invoke(this, error);
                        break;
                    case MessageType.Tick:

                        var tick = json.ToObject<TickerDataV2>();
                        _tickerChanged?.Invoke(this, tick);

                        break;
                    case MessageType.Trade:
                        var marketTrade = json.ToObject<MarketTradeDataV2>();
                        _marketTradeChanged?.Invoke(this, marketTrade);

                        break;
                    case MessageType.OrderBook:
                        var orderBook = json.ToObject<OrderBookDataV2>();
                        _orderBookChanged?.Invoke(this, orderBook);

                        break;
                    case MessageType.OrderChange:
                        var orderData = json.ToObject<OrderChangeDataV2>();
                        _orderChanged?.Invoke(this, orderData);
                        break;
                    case MessageType.FundChange:
                        var fundChange = json.ToObject<FundTransferDataV2>();
                        _fundTransferred?.Invoke(this, fundChange);
                        break;
                }
            }
            catch (Exception ex)
            {
                _onError?.Invoke(this, ex);
            }
        }

        private void _socket_OnSendFailed(string data, Exception ex)
        {
            _sendFailed?.Invoke(this, new SocketEventArgs { });
        }

        private void _socket_OnError(Exception ex)
        {
            _onError?.Invoke(this, ex);
        }

    }
}
