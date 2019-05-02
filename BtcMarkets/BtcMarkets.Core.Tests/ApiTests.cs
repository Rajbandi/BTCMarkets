using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtcMarkets.Core;
using BtcMarkets.Core.Api;
using BtcMarkets.Core.Api.Contracts;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using BtcMarkets.Core.Helpers;
using System.Threading.Tasks;

namespace BtcMarkets.Core.Tests
{
    [TestClass]
    public class ApiTests
    {
        private readonly BtcMarketsClient _client;
        
        public ApiTests()
        {
            _client = new BtcMarketsClient(new ApiSettings
            {
                BaseUrl = "https://api.btcmarkets.net",
                ApiKey = "c313c21c-3908-45ed-93e4-71c6b9f841ab",
                Secret = "9poMsaAxsFskxhkL7IWCEaX5GKMxPPhippkshRjZ/dFzk33+4xMh/CTvgLaIK3UE6JpLQTpK/OqgxfF87DEeEg=="
            });
        }

        [TestMethod]
        public void TestMarketTick()
        {
            var tick = _client.Api.GetMarketTick("BTC","AUD").Result;
            Assert.IsNotNull(tick);
            var json = tick.ToJson();
            //Debug.WriteLine(json);
            Console.WriteLine(json);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(json));
        }

        [TestMethod]
        public void TestMarketTrades()
        {
            var response = _client.Api.GetMarketTrades("BTC","AUD").Result;
            Assert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response);
            //Debug.WriteLine(json);
            Console.WriteLine(json);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(json));

        }

        [TestMethod]
        public void TestMarketOrderBook()
        {
            var response = _client.Api.GetMarketOrderBook("BTC", "AUD").Result;
            Assert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response);
           // Debug.WriteLine(json);
            Console.WriteLine(json);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(json));

        }

        [TestMethod]
        public void TestActiveMarkets()
        {
            var response = _client.Api.GetActiveMarkets().Result;
            Assert.IsNotNull(response);

            var json = JsonConvert.SerializeObject(response);
           // Debug.WriteLine(json);
            Debug.WriteLine(json);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(json));

        }
        [TestMethod]
        public void TestActiveMarketList()
        {
            var response = _client.Api.GetActiveMarkets().Result;
            Assert.IsNotNull(response);

            var ticks = new List<MarketTick>();
            foreach(var market in response.Markets)
            {
                var tick = _client.Api.GetMarketTick(market.Instrument, market.Currency);
                ticks.Add(tick.Result);
            }
            var json = JsonConvert.SerializeObject(ticks);

            // Debug.WriteLine(json);
            Debug.WriteLine(json);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(json));

        }
        [TestMethod]
        public void TestAccountBalance()
        {
            var response = _client.Api.GetAccountBalance().Result;
            Assert.IsNotNull(response);

            if (response != null)
            {
                var json = JsonConvert.SerializeObject(response);
                // Debug.WriteLine(json);
                Console.WriteLine(json);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json));
            }

        }

        [TestMethod]
        public void TestTradingFee()
        {
            var response = _client.Api.GetTradingFee("BTC", "AUD").Result;
            Assert.IsNotNull(response);

            if (response != null)
            {
                var json = JsonConvert.SerializeObject(response);
                // Debug.WriteLine(json);
                Console.WriteLine(json);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json));
            }

        }

        [TestMethod]
        public void TestOrderHistory()
        {
            var response = _client.Api.GetOrderHistory(new OrderHistoryRequest
            {
                Currency = "AUD",
                Instrument = "BTC",
                Limit = 200
            }).Result;
            //Console.WriteLine(response);
            Assert.IsNotNull(response);

            if (response != null)
            {
                var json = JsonConvert.SerializeObject(response);
                // Debug.WriteLine(json);
                Console.WriteLine(json);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json));
            }

        }

        [TestMethod]
        public void TestOrderTradeHistory()
        {
            var response = _client.Api.GetOrderTradeHistory(new OrderTradeHistoryRequest
            {
                Currency = "AUD",
                Instrument = "BTC",
                Limit = 200
            }).Result;
            //Console.WriteLine(response);
            Assert.IsNotNull(response);

            ProcessResponse(response);

        }
        [TestMethod]
        public void TestGetOpenOrders()
        {
            var response = _client.Api.GetOpenOrders(new OrderHistoryRequest
            {
                Currency = "AUD",
                Instrument = "BTC",
                Limit = 200
            }).Result;
            //Console.WriteLine(response);
            Assert.IsNotNull(response);

            ProcessResponse(response);

        }
        [TestMethod]
        public void TestGetOpenOrdersV2()
        {
            
                var response = _client.Api.GetOpenOrdersV2().Result;
                //Console.WriteLine(response);
                Assert.IsNotNull(response);

                ProcessResponse(response);

         

        }

        [TestMethod]
        public void TestCreateOrder()
        {
            //var response = _client.Api.CreateOrder(new CreateOrderRequest
            //{
            //    Instrument = "BTC",
            //    Currency = "AUD",
            //    OrderSide = "Ask",
            //    ClientRequestId = "abc-cdf-11111",
            //    Ordertype = "Limit",
            //    Price = ApiHelper.ToLongValue(9400.00),
            //    Volume = ApiHelper.ToLongValue(0.01)
            //}).Result;
            ////Console.WriteLine(response);
            //Assert.IsNotNull(response);

            //ProcessResponse(response);

        }

        [TestMethod]
        public void TestCancelOrders()
        {
            var response = _client.Api.CancelOrder(new CancelOrderRequest
            {
                OrderIds =  new List<long>
                {

                    2379872240,
                    2379875538,
                    2379876321,

                }
            }).Result;
            //Console.WriteLine(response);
            Assert.IsNotNull(response);

            ProcessResponse(response);

        }

        private void ProcessResponse(object response)
        {
            if (response != null)
            {
                var json = JsonConvert.SerializeObject(response);
                // Debug.WriteLine(json);
                Console.WriteLine(json);
                Assert.IsTrue(!string.IsNullOrWhiteSpace(json));
            }
        }
    }
}
