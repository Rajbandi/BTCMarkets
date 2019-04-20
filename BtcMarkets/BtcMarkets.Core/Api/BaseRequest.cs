using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core
{
    public abstract class BaseRequest
    {
        public string Path { get; set; }

        public bool IsPost { get; set; }

        public virtual string GetRequestData()
        {
            return string.Empty;
        }


    }
}
