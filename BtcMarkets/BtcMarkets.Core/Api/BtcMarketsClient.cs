using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BtcMarkets.Core.Api.Contracts;
using Newtonsoft.Json;
using Refit;

namespace BtcMarkets.Core.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class BtcMarketsClient
    {
        readonly ApiSettings _settings;
        public BtcMarketsClient(ApiSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings),"Settings is required");
            }
            if(string.IsNullOrWhiteSpace(settings.BaseUrl))
            {
                throw new ArgumentNullException(nameof(settings.ApiKey), "BaseUrl is required");
            }

            //if (string.IsNullOrWhiteSpace(settings.ApiKey))
            //{
            //    throw new ArgumentNullException(nameof(settings.ApiKey), "ApiKey is required");
            //}

            //if (string.IsNullOrWhiteSpace(settings.Secret))
            //{
            //    throw new ArgumentNullException(nameof(settings.Secret), "Secret is required");
            //}

            _settings = settings;

        }

        public ApiSettings Settings => _settings;

        public async Task<AccountBalanceResponse> GetAccountBalance(IBtcMarketsApi api = null)
        {
            var responseObj = new AccountBalanceResponse
            {
                Success = true
            };

            try
            {
                if(api == null)
                {
                    api = Api;
                }
                HttpResponseMessage response = await api.GetAccountBalanceRaw();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var balances = JsonConvert.DeserializeObject<List<AccountBalance>>(content);
                        responseObj.Balances = balances;
                    }
                    catch (Exception)
                    {
                        var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(content);
                        if (baseResponse != null)
                        {
                            responseObj.Success = baseResponse.Success;
                            responseObj.ErrorCode = baseResponse.ErrorCode;
                            responseObj.ErrorMessage = baseResponse.ErrorMessage;
                        }
                    }
                }
                else
                {
                    responseObj.Success = false;
                    responseObj.ErrorCode = 101;
                    responseObj.ErrorMessage = "Remote api returned invalid status code while retrieving balances";
                }

            }
            catch (Exception ex)
            {
                responseObj.Success = false;
                responseObj.ErrorCode = 100;
                responseObj.ErrorMessage = ex.ToString();
            }
            return responseObj;
        }

        public async Task<TradingFee> GetTradingFee(string instrument, string currency)
        {
            var responseObj = new TradingFee
            {
                Success = true
            };

            try
            {
                HttpResponseMessage response = await Api.GetTradingFeeRaw(instrument, currency);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var baseResponse = JsonConvert.DeserializeObject<TradingFee>(content);
                    if (baseResponse != null)
                    {
                        responseObj.Success = baseResponse.Success;
                        responseObj.ErrorCode = baseResponse.ErrorCode;
                        responseObj.ErrorMessage = baseResponse.ErrorMessage;
                        responseObj.TradingFeeRate = baseResponse.TradingFeeRate;
                        responseObj.Volume30Day = baseResponse.Volume30Day;
                    }

                }
                else
                {
                    responseObj.Success = false;
                    responseObj.ErrorCode = 101;
                    responseObj.ErrorMessage = "Remote api returned invalid status code while retrieving trading fee";
                }

            }
            catch (Exception ex)
            {
                responseObj.Success = false;
                responseObj.ErrorCode = 100;
                responseObj.ErrorMessage = ex.ToString();
            }
            return responseObj;
        }

        public async Task<bool> CheckApiCredentials(string apiKey, string secret)
        {
            var settings = new ApiSettings();
            settings.BaseUrl = _settings.BaseUrl;
            settings.ApiKey = apiKey;
            settings.Secret = secret;

            var authHandler = new AuthenticationHandler(settings);
            var client = new HttpClient(authHandler)
            {
                BaseAddress = new Uri(settings.BaseUrl)
            };
            var api = RestService.For<IBtcMarketsApi>(client);
            var response = await GetAccountBalance(api);
            var isValid = response.Success.HasValue ? response.Success.Value : false;
            return isValid;
        }

        private IBtcMarketsApi _api;
        public IBtcMarketsApi Api
        {
            get
            {
                if (_api == null)
                {
                    var authHandler = new AuthenticationHandler(_settings);
                    var client = new HttpClient(authHandler)
                    {
                        BaseAddress = new Uri(_settings.BaseUrl)
                    };
                    _api = RestService.For<IBtcMarketsApi>(client);
                }
                return _api;
            }
        }
    }
}
