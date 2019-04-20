using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BtcMarkets.Wallet.Models;

namespace BtcMarkets.Wallet.Droid.Helpers
{
    public static class ViewHelper
    {
        public static GradientDrawable.Orientation GetGradientDirection(GradientDirection direction)
        {
            GradientDrawable.Orientation orientation = GradientDrawable.Orientation.BrTl;
            switch (direction)
            {
                case GradientDirection.TopBottom:
                    orientation = GradientDrawable.Orientation.TopBottom;
                    break;
                case GradientDirection.BottomTop:
                    orientation = GradientDrawable.Orientation.BottomTop;
                    break;
                case GradientDirection.LeftRight:
                    orientation = GradientDrawable.Orientation.LeftRight;
                    break;
                case GradientDirection.RightLeft:
                    orientation = GradientDrawable.Orientation.RightLeft;
                    break;
                case GradientDirection.BottomLeftTopRight:
                    orientation = GradientDrawable.Orientation.BlTr;
                    break;
                case GradientDirection.TopLeftBottomRight:
                    orientation = GradientDrawable.Orientation.TlBr;
                    break;
                case GradientDirection.TopRightBottomLeft:
                    orientation = GradientDrawable.Orientation.TrBl;
                    break;
                default:
                    orientation = GradientDrawable.Orientation.BrTl;
                    break;


            }

            return orientation;
        }
    }
}