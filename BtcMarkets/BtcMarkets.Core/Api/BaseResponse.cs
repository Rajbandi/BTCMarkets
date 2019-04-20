using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core
{
    public class BaseResponse<T> 
    {
        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        public T Data { get; set; }
    }
}
