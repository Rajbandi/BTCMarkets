using BtcMarkets.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public class DataGridColumn : BaseBindableObject
    {
        public static readonly BindableProperty HeaderProperty = BindableProperty.Create(nameof(Header), typeof(string), typeof(DataGridColumn), default(string), BindingMode.OneWay);
        public string Header
        {
            get
            {
                return (string)GetValue(HeaderProperty);
            }
            set
            {
                SetValue(HeaderProperty, value);
            }
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(DataGridColumn), default(string), BindingMode.OneWay);
        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly BindableProperty LengthProperty = BindableProperty.Create(nameof(Length), typeof(double), typeof(DataGridColumn), default(double), BindingMode.OneWay);
        public double Length
        {
            get
            {
                return (double)GetValue(LengthProperty);
            }
            set
            {
                SetValue(LengthProperty, value);
            }
        }
    }
   
}
