﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Models
{
    public class MarketTradePair : BaseBindableObject
    {
        private string _pair;
        public string Pair
        {
            get => _pair;
            set => SetProperty(ref _pair, value);
        }

        private Style _style;

        public Style Style
        {
            get => _style;
            set => SetProperty(ref _style, value);
        }
        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
    }
}
