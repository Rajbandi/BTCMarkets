using BtcMarkets.Wallet.Models;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Effects
{
    public class GradientEffect : RoutingEffect
    {
        public GradientDirection Direction { get; set; }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }

        public int? Elevation { get; set; }
        public GradientEffect() : base("BtcMarkets.GradientEffect")
        {
        }
    }
}
