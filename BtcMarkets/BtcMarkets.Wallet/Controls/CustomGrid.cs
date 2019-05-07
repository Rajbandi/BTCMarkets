using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.Controls
{
    public class CustomGrid : Grid
    {
        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                base.OnSizeAllocated(width, height);
            }
            catch(Exception)
            {

            }

        }
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            try
            {
                base.LayoutChildren(x, y, width, height);
            }
            catch(Exception)
            {

            }
        }
        protected override void OnChildMeasureInvalidated()
        {
            try
            {
                base.OnChildMeasureInvalidated();
            }
            catch(Exception)
            {

            }
        }
    }
}
