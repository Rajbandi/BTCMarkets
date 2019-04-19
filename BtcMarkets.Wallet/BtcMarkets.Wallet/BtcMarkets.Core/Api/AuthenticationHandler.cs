using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BtcMarkets.Core.Helpers;

namespace BtcMarkets.Core.Api
{
    /// <summary>
    /// Authentication Handler to add custom headers by intercepting each get and post request 
    /// </summary>
    class AuthenticationHandler : HttpClientHandler
    {
        private readonly ApiSettings _settings;

        public AuthenticationHandler(ApiSettings settings)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var buffer = new StringBuilder();
            var timestamp = ApiHelper.GetTimeStamp();
            var action = request.RequestUri.PathAndQuery;

            buffer.Append(action + "\n");
            buffer.Append(timestamp + "\n");

            string postData = string.Empty;

            if (request.Method == HttpMethod.Post)
            {
                postData = request.Content.ReadAsStringAsync().Result;
            }

            if (!string.IsNullOrWhiteSpace(postData))
            {
                buffer.Append(postData);
            }

            var data = buffer.ToString();
            if (!string.IsNullOrWhiteSpace(data) && !string.IsNullOrWhiteSpace(_settings.Secret))
            {
                var signature = ApiHelper.SignData(data, _settings.Secret);
                request.Headers.Add(ApiConstants.Signature, signature);
            }

            if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                request.Headers.Add(ApiConstants.ApiKey, _settings.ApiKey);
            }

            request.Headers.Add(ApiConstants.TimeStamp, timestamp);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
