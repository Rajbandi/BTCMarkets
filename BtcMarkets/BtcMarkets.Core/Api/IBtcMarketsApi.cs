using System;
using BtcMarkets.Core.Api.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
namespace BtcMarkets.Core.Api
{
    /// <summary>
    /// IBtcMarketsApi
    /// </summary>
    public interface IBtcMarketsApi
    {
        #region  Market Related

        [Get("/market/{instrument}/{currency}/tick")]
        Task<MarketTick> GetMarketTick(string instrument, string currency);

        [Get("/market/{instrument}/{currency}/tick")]
        Task<HttpResponseMessage> GetMarketTickRaw(string instrument, string currency);

        [Get("/market/{instrument}/{currency}/orderbook")]
        Task<MarketOrderBook> GetMarketOrderBook(string instrument, string currency);

        [Get("/market/{instrument}/{currency}/orderbook")]
        Task<HttpResponseMessage> GetMarketOrderBookRaw(string instrument, string currency);

        [Get("/market/{instrument}/{currency}/trades")]
        Task<IList<MarketTrade>> GetMarketTrades(string instrument, string currency);

        [Get("/market/{instrument}/{currency}/trades")]
        Task<HttpResponseMessage> GetMarketTradesRaw(string instrument, string currency);

        [Get("/v2/market/active")]
        Task<ActiveMarkets> GetActiveMarkets();

        [Get("/v2/market/active")]
        Task<HttpResponseMessage> GetActiveMarketsRaw();

        [Get("/v2/market/{instrument}/{currency}/trades")]
        Task<MarketTradesV2> GetMarketTradesV2(string instrument, string currency, MarketParams query = null);

        [Get("/v2/market/{instrument}/{currency}/trades")]
        Task<HttpResponseMessage> GetMarketTradesV2Raw(string instrument, string currency, MarketParams query = null);

        [Get("/v2/market/{instrument}/{currency}/tickByTime/{time}")]
        Task<MarketHistoryV2> GetMarketHistory(string instrument, string currency, string time, MarketParams query = null);

        [Get("/v2/market/{instrument}/{currency}/tickByTime/{time}")]
        Task<HttpResponseMessage> GetMarketHistoryRaw(string instrument, string currency, string time, MarketParams query = null);

        #endregion

        #region  Account Related

        [Get("/account/balance")]
        Task<IList<AccountBalance>> GetAccountBalance();

        [Get("/account/balance")]
        Task<HttpResponseMessage> GetAccountBalanceRaw();

        [Get("/account/{instrument}/{currency}/tradingfee")]
        Task<TradingFee> GetTradingFee(string instrument, string currency);

        [Get("/account/{instrument}/{currency}/tradingfee")]
        Task<HttpResponseMessage> GetTradingFeeRaw(string instrument, string currency);
        #endregion

        #region Funds related

        [Get("/fundtransfer/history")]
        Task<FundTransferResponse> GetFundTransferHistory(MarketParams query = null);
        #endregion

        #region  Order Related

        [Post("/order/create")]
        Task<CreateOrderResponse> CreateOrder([Body] CreateOrderRequest request);

        [Post("/order/cancel")]
        Task<CancelOrderResponse> CancelOrder([Body] CancelOrderRequest request);

        [Post("/order/history")]
        Task<OrderHistoryResponse> GetOrderHistory([Body] OrderHistoryRequest request);

        [Get("/v2/order/history/{instrument}/{currency}")]
        Task<OrderHistoryResponse> GetOrderHistoryV2(string instrument, string currency, MarketParams query = null);

        [Post("/order/open")]
        Task<OrderHistoryResponse> GetOpenOrders([Body] OrderHistoryRequest request);

        [Post("/order/trade/history")]
        Task<OrderTradeHistoryResponse> GetOrderTradeHistory([Body] OrderTradeHistoryRequest request);

        [Post("/order/detail")]
        Task<OrderDetailResponse> GetOrderTradeHistory([Body] OrderDetailRequest request);

        [Post("/v2/order/open")]
        Task<OrderHistoryResponse> GetOpenOrdersV2([Body] OrderHistoryRequest request);

        [Get("/v2/order/open")]
        Task<OrderHistoryResponse> GetOpenOrdersV2();


        [Get("/v2/order/open")]
        Task<HttpResponseMessage> GetOpenOrdersV2Raw();

        [Get("/v2/order/open/{instrument}/{currency}")]
        Task<OrderHistoryResponse> GetOpenOrdersV2(string instrument, string currency);

        #endregion

    }
}
