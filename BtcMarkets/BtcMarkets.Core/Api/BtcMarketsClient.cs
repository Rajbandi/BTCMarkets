using System;
using System.Net.Http;
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
