using System;
using BtcMarkets.Core.Api.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BtcMarkets.Core.Api
{
    /// <summary>
    /// IBtcMarketsApi
    /// </summary>
    public interface IBtcMarketApi
    {
        #region  Market Related

        Task<BaseResponse<MarketTick>> GetMarketTick(string instrument, string currency);

        Task<BaseResponse<MarketOrderBook>> GetMarketOrderBook(string instrument, string currency);

        Task<BaseResponse<IList<MarketTrade>>> GetMarketTrades(string instrument, string currency);

        Task<BaseResponse<ActiveMarkets>> GetActiveMarkets();

        Task<BaseResponse<IList<MarketTradesV2>>> GetMarketTrades(string instrument, string currency, MarketParams query = null);

        #endregion

        #region  Account Related

        Task<BaseResponse<IList<AccountBalance>>> GetAccountBalance();

        Task<BaseResponse<TradingFee>> GetTradingFee(string instrument, string currency);

        #endregion

        #region  Order Related

        Task<BaseResponse<CreateOrderResponse>> CreateOrder( CreateOrderRequest request);

        Task<BaseResponse<CancelOrderResponse>> CancelOrder( CancelOrderRequest request);

        Task<BaseResponse<OrderHistoryResponse>> GetOrderHistory( OrderHistoryRequest request);

        Task<BaseResponse<OrderHistoryResponse>> GetOpenOrders( OrderHistoryRequest request);

        Task<BaseResponse<OrderTradeHistoryResponse>> GetOrderTradeHistory( OrderTradeHistoryRequest request);

        Task<BaseResponse<OrderDetailResponse>> GetOrderTradeHistory( OrderDetailRequest request);


        #endregion

    }
}
