using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public class MarketValue : Label
    {
        private readonly Span _changeIcon;
        private readonly Span _changeValue;
      
        public MarketValue()
        {
            ValueView = new Label();
            ValueView.BindingContext = this;

            ValueView.SetBinding(StyleProperty, "ValueStyle");

            _changeIcon = new Span();
            _changeValue = new Span();
            
        }

        public event EventHandler<TextChangedEventArgs> ValueChanged;

        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(MarketValue), null, BindingMode.TwoWay, propertyChanged: OnValueChanged);
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty ValueStyleProperty = BindableProperty.Create("ValueStyle", typeof(Style), typeof(MarketValue), null, BindingMode.TwoWay);
        public Style ValueStyle
        {
            get => (Style)GetValue(ValueStyleProperty);
            set => SetValue(ValueStyleProperty, value);
        }

        public static readonly BindableProperty IsChangeViewVisibleProperty = BindableProperty.Create("IsChangeViewVisible", typeof(bool), typeof(MarketValue), null, BindingMode.TwoWay);
        public string IsChangeViewVisible
        {
            get => (string)GetValue(IsChangeViewVisibleProperty);
            set => SetValue(IsChangeViewVisibleProperty, value);
        }

        public Label ValueView { get; }

        async static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            
            var view = (MarketValue)bindable;
            if (view != null)
            {
                var label = view.ValueView;
                

                var color = label.TextColor;

                label.Text = (string)newValue;

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

               
                label.TextColor = ((Color)Application.Current.Resources["DefaultTextColor"]);

                view.ValueChanged?.Invoke(label, new TextChangedEventArgs((string)oldValue, (string)newValue));
            }
        }
    }
}
