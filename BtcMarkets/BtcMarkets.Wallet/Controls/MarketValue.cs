using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public class MarketValue : Label
    {
        
        private readonly Span _changeText;
        private readonly Span _symbol;
        private readonly Span _changeValue;
 

        public Span ValueView => _changeValue;
        public Span SymbolView => _symbol;

        public Span ChangeTextView => _changeValue;

        public MarketValue()
        {
            _changeValue = new Span();
            _symbol = new Span();
            _changeText = new Span();
           
            var formattedString = new FormattedString();
            formattedString.Spans.Add(_symbol);
            formattedString.Spans.Add(_changeValue);
            formattedString.Spans.Add(_changeText);


            this.FormattedText = formattedString;

       

        }

        public event EventHandler<TextChangedEventArgs> ValueChanged;

        public static readonly BindableProperty SymbolProperty = BindableProperty.Create("Symbol", typeof(string), typeof(MarketValue), null, BindingMode.TwoWay, propertyChanged:OnSymbolChanged);
        public string Symbol
        {
            get => (string)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        private static void OnSymbolChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var view = (MarketValue)bindable;
                if(view != null)
                {
                    view.SymbolView.Text = (string)newValue;
                }
            }
            catch(Exception)
            {

            }
        }

       

        public static readonly BindableProperty SymbolStyleProperty = BindableProperty.Create("SymbolStyle", typeof(Style), typeof(MarketValue), null, BindingMode.TwoWay, propertyChanged:OnSymbolStyleChanged);

        private static void OnSymbolStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var view = (MarketValue)bindable;
                if (view != null)
                {
                    view.SymbolView.Style = (Style)newValue;
                }
            }
            catch (Exception)
            {

            }
        }

        public Style SymbolStyle
        {
            get => (Style)GetValue(SymbolStyleProperty);
            set => SetValue(SymbolStyleProperty, value);
        }

        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(MarketValue), null, BindingMode.TwoWay, propertyChanged: OnValueChanged);
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        public static readonly BindableProperty ValueStyleProperty = BindableProperty.Create("ValueStyle", typeof(Style), typeof(MarketValue), null, BindingMode.TwoWay, propertyChanged:OnValueStyleChanged);

        private static void OnValueStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var view = (MarketValue)bindable;
                if (view != null)
                {
                    view.ValueView.Style = (Style)newValue;
              

                }
            }
            catch (Exception)
            {

            }
        }

        public Style ValueStyle
        {
            get => (Style)GetValue(ValueStyleProperty);
            set => SetValue(ValueStyleProperty, value);
        }

        public static readonly BindableProperty IsSymbolVisibleProperty = BindableProperty.Create("IsSymbolVisible", typeof(bool), typeof(MarketValue), false, BindingMode.TwoWay,propertyChanged: OnIsSymbolVisibleChanged);

        private static void OnIsSymbolVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            try
            {
                var view = (MarketValue)bindable;
                if (view != null)
                {
                    //view.SymbolView.IsSet = (string)newValue;
                }
            }
            catch (Exception)
            {

            }
        }

        public bool IsSymbolVisible
        {
            get => (bool)GetValue(IsSymbolVisibleProperty);
            set => SetValue(IsSymbolVisibleProperty, value);
        }

        public static readonly BindableProperty ChangeTextProperty = BindableProperty.Create("ChangeText", typeof(string), typeof(MarketValue), null, BindingMode.TwoWay);
        public string ChangeText
        {
            get => (string)GetValue(ChangeTextProperty);
            set => SetValue(ChangeTextProperty, value);
        }

        public static readonly BindableProperty ChangeTextStyleProperty = BindableProperty.Create("ChangeTextStyle", typeof(Style), typeof(MarketValue), null, BindingMode.TwoWay);
        public Style ChangeTextStyle
        {
            get => (Style)GetValue(ChangeTextStyleProperty);
            set => SetValue(ChangeTextStyleProperty, value);
        }


        public static readonly BindableProperty IsChangeTextVisibleProperty = BindableProperty.Create("IsChangeTextVisible", typeof(bool), typeof(MarketValue), false, BindingMode.TwoWay);
        public bool IsChangeTextVisible
        {
            get => (bool)GetValue(IsChangeTextVisibleProperty);
            set => SetValue(IsChangeTextVisibleProperty, value);
        }

        async static void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {

            try
            {

                var view = (MarketValue)bindable;
                if (view != null)
                {
                

                    double previousVal, newVal;

                    double.TryParse((string)oldValue, out previousVal);
                    double.TryParse((string)newValue, out newVal);
                    
                    var label = view.ValueView;
                    var color = label.TextColor;

                    label.Text = (string)newValue;
                    var change = newVal - previousVal;
                    if (change != 0 && oldValue != null)
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
                        await Task.Delay(500);
                    }
                    label.TextColor = ((Color)Application.Current.Resources["DefaultTextColor"]);
                    view.ValueChanged?.Invoke(label, new TextChangedEventArgs((string)oldValue, (string)newValue));
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
