using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Views
{
    [ContentProperty("PageContent")]
    public class BasePage : ContentPage
    {
        public static readonly BindableProperty PageContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentPage), null);

        public View PageContent
        {
            get
            {
                return base.Content;
            }
            set
            {
                base.Content = value;
            }
        }
    }
}
