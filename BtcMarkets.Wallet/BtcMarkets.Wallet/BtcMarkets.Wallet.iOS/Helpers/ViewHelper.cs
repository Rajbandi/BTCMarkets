using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BtcMarkets.Wallet.Models;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace BtcMarkets.Wallet.iOS.Helpers
{
    public static class ViewHelper
    {
        public static CGPoint[] GetGradientPoints(GradientDirection direction)
        {
            var points = new CGPoint[2];
            switch (direction)
            {
                case GradientDirection.BottomTop:
                    points[0] = new CGPoint(0.5, 1.0);
                    points[1] = new CGPoint(0.5, 0.0);
                    break;
                case GradientDirection.LeftRight:
                    points[0] = new CGPoint(0.0, 0.5);
                    points[1] = new CGPoint(1.0, 0.5);
                    break;
                case GradientDirection.RightLeft:
                    points[0] = new CGPoint(1.0, 0.5);
                    points[1] = new CGPoint(0.0, 0.5);
                    break;
                case GradientDirection.TopBottom:
                    points[0] = new CGPoint(0.5, 0.0);
                    points[1] = new CGPoint(0.5, 1.0);
                    break;
                case GradientDirection.TopLeftBottomRight:
                    points[0] = new CGPoint(0.0, 0.0);
                    points[1] = new CGPoint(1.0, 1.0);
                    break;
                case GradientDirection.TopRightBottomLeft:
                    points[0] = new CGPoint(1.0, 0.0);
                    points[1] = new CGPoint(0.0, 1.0);
                    break;
                case GradientDirection.BottomLeftTopRight:
                    points[0] = new CGPoint(0.0, 1.0);
                    points[1] = new CGPoint(1.0, 0.0);
                    break;
                default:
                    points[0] = new CGPoint(1.0, 1.0);
                    points[1] = new CGPoint(0.0, 0.0);
                    break;


            }

            return points;
        }
       
        
    }
}