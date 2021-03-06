﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BtcMarkets.Core
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; }

        public string SocketUrl { get; set; }

        public string SocketV2Url { get; set; }

        public string ApiKey { get; set; }

        public string Secret { get; set; }
    }
}
