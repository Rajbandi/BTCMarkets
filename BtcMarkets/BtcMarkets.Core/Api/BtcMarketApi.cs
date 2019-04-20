using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BtcMarkets.Core.Api.Contracts;
using Newtonsoft.Json;

namespace BtcMarkets.Core.Api
{
    public class BtcMarketApi : IBtcMarketApi
    {
        private ApiSettings _settings;
        private HttpClient _client;
        public BtcMarketApi(ApiSettings settings)
        {
            _settings = settings;
            var authHandler = new AuthenticationHandler(_settings);
            _client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(_settings.BaseUrl)
            };
        }

        public Task<BaseResponse<CancelOrderResponse>> CancelOrder(CancelOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<CreateOrderResponse>> CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IList<AccountBalance>>> GetAccountBalance()
        {
            throw new NotImplementedException();
        }


        public async Task<BaseResponse<T>> GetData<T>(string path)
        {
            var response = new BaseResponse<T>();
            try
            {
                using (HttpResponseMessage httpResponse = await _client.GetAsync(path))
                {


                    if (httpResponse.IsSuccessStatusCode)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            response.Data = JsonConvert.DeserializeObject<T>(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = "100";
                response.ErrorDescription = ex.ToString();
            }
            return response;
        }

        public async Task<BaseResponse<ActiveMarkets>> GetActiveMarkets()
        {
            var response = await GetData<ActiveMarkets>("/v2/market/active");
            return response;
        }

        public async Task<BaseResponse<MarketTick>> GetMarketTick(string instrument, string currency)
        {
            var path = $"/market/{instrument}/{currency}/tick";
            var response = await GetData<MarketTick>(path);
            return response;
        }


        public Task<BaseResponse<MarketOrderBook>> GetMarketOrderBook(string instrument, string currency)
        {
            throw new NotImplementedException();
        }

       
        public Task<BaseResponse<IList<MarketTrade>>> GetMarketTrades(string instrument, string currency)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<IList<MarketTradesV2>>> GetMarketTrades(string instrument, string currency, MarketParams query = null)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<OrderHistoryResponse>> GetOpenOrders(OrderHistoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<OrderHistoryResponse>> GetOrderHistory(OrderHistoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<OrderTradeHistoryResponse>> GetOrderTradeHistory(OrderTradeHistoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<OrderDetailResponse>> GetOrderTradeHistory(OrderDetailRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<TradingFee>> GetTradingFee(string instrument, string currency)
        {
            throw new NotImplementedException();
        }
    }
}
