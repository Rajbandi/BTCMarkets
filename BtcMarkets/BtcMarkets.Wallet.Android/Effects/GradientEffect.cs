using System;
using System.ComponentModel;
using System.Linq;
using Android.Graphics.Drawables;
using BtcMarkets.Wallet.Droid.Effects;
using BtcMarkets.Wallet.Droid.Helpers;
using BtcMarkets.Wallet.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("BtcMarkets")]
[assembly: ExportEffect(typeof(GradientEffect), "GradientEffect")]
namespace BtcMarkets.Wallet.Droid.Effects
{
    public class GradientEffect : PlatformEffect
    {
        private GradientDrawable _background;
        private Wallet.Effects.GradientEffect _effect;
        protected override void OnAttached()
        {
            try
            {
                _effect = (Wallet.Effects.GradientEffect)Element.Effects.FirstOrDefault(e => e is Wallet.Effects.GradientEffect);
                if (_effect != null)
                {
                    _background = new GradientDrawable(ViewHelper.GetGradientDirection(_effect.Direction),
                        new int[] { _effect.StartColor.ToAndroid(), _effect.EndColor.ToAndroid() });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            try
            {
                if (_background != null)
                {
                    var control = Control ?? Container;
                    if (control != null && control.Background != _background)
                    {
                       control.Background = _background;
                        //    control.SetBackgroundColor(ResourceHelper.PrimaryColor.ToAndroid());
                        if(_effect.Elevation.HasValue)
                            control.Elevation = _effect.Elevation.Value;
                      
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
}