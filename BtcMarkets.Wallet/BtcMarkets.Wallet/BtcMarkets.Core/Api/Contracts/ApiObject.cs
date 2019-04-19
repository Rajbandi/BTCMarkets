using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core.Api.Contracts
{
    public class ApiObject
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
