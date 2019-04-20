using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BtcMarkets.Wallet.Helpers;
using BtcMarkets.Wallet.iOS.Effects;
using BtcMarkets.Wallet.iOS.Helpers;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("BtcMarkets")]
[assembly: ExportEffect(typeof(GradientEffect), "GradientEffect")]
namespace BtcMarkets.Wallet.iOS.Effects
{
    public class GradientEffect : PlatformEffect
    {
        private Wallet.Effects.GradientEffect _effect;
        private CAGradientLayer _layer;
        protected override void OnAttached()
        {
            _effect = (Wallet.Effects.GradientEffect)Element.Effects.FirstOrDefault(e => e is Wallet.Effects.GradientEffect);

        }

        protected override void OnDetached()
        {
            throw new NotImplementedException();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            var control = Control ?? Container;
            if (control != null)
            {
                control.BackgroundColor = _effect.StartColor.ToUIColor();

                //control.Layer.ShadowColor = UIColor.Black.CGColor;
                //control.Layer.ShadowOffset = new CGSize(0, 0);
              //  control.Layer.ShadowRadius = 3;
                //control.Layer.ShadowOpacity = 1;
            }
        }
    }
}