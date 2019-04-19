using BtcMarkets.Wallet.Controls;
using BtcMarkets.Wallet.iOS.Renderers;
using CoreGraphics;
using Foundation;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRenderer))]
namespace BtcMarkets.Wallet.iOS.Renderers
{
    public class ExtendedUIWebViewDelegate : UIWebViewDelegate
    {
        CustomWebViewRenderer webViewRenderer;

        public ExtendedUIWebViewDelegate(CustomWebViewRenderer _webViewRenderer = null)
        {
            webViewRenderer = _webViewRenderer ?? new CustomWebViewRenderer();
        }

        public override void LoadStarted(UIWebView webView)
        {

            try
            {
                var wv = webViewRenderer.Element as CustomWebView;
                if (wv != null)
                {
                    wv.IsLoading = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void LoadFailed(UIWebView webView, NSError error)
        {
            try
            {
                var wv = webViewRenderer.Element as CustomWebView;
                if (wv != null)
                {
                    wv.IsLoading = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public override async void LoadingFinished(UIWebView webView)
        {
            try
            {
                var wv = webViewRenderer.Element as CustomWebView;
                if (wv != null)
                {
                    await System.Threading.Tasks.Task.Delay(100); // wait here till content is rendered
                    wv.IsLoading = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class CustomWebViewRenderer : WebViewRenderer
    {
        private CustomWebView _webView;
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _webView = this.Element as CustomWebView;
            Delegate = new ExtendedUIWebViewDelegate(this);
        }
    }
     
}