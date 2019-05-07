using BtcMarkets.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public class LabelText : Label
    {
        public event EventHandler<TextChangedEventArgs> TextChanged;

        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(LabelText), null, BindingMode.TwoWay, propertyChanged: OnTextChanged);

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        
        static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Task.Run(async () =>
            {


                var label = (LabelText)bindable;
                if (label != null)
                {
                    var color = label.TextColor;

                    label.Text = (string)newValue;
                    label.TextChanged?.Invoke(label, new TextChangedEventArgs((string)oldValue, (string)newValue));

                 
                    var previousVal = Convert.ToDouble((string)oldValue);
                    var newVal = Convert.ToDouble((string)newValue);
                    var change = newVal - previousVal;


                    if (change != 0)
                    {
                        Color newColor = color;
                        if (change > 0)
                        {
                            newColor = Color.Green;
                        }
                        else
                            if (change < 0)
                        {
                            newColor = Color.Red;
                        }

                        label.TextColor = newColor;
                        await Task.Delay(2000);
                        
                    }

                    if(label.TextColor  != color)
                        label.TextColor = color;

                }
            });
        }

    }
}
