using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace WowonderPhone.Droid
{
    public class AndroidBug5497WorkaroundForXamarinAndroid
    {
        private readonly View mChildOfContent;
        private int usableHeightPrevious;
        private FrameLayout.LayoutParams frameLayoutParams;

        public static void assistActivity(Activity activity, IWindowManager windowManager)
        {
            new AndroidBug5497WorkaroundForXamarinAndroid(activity, windowManager);
        }

        private AndroidBug5497WorkaroundForXamarinAndroid(Activity activity, IWindowManager windowManager)
        {

            var softButtonsHeight = getSoftbuttonsbarHeight(windowManager);

            var content = (FrameLayout)activity.FindViewById(Android.Resource.Id.Content);
            mChildOfContent = content.GetChildAt(0);
            var vto = mChildOfContent.ViewTreeObserver;
            vto.GlobalLayout += (sender, e) => possiblyResizeChildOfContent(softButtonsHeight);
            frameLayoutParams = (FrameLayout.LayoutParams)mChildOfContent.LayoutParameters;
        }

        private void possiblyResizeChildOfContent(int softButtonsHeight)
        {
            var usableHeightNow = computeUsableHeight();
            if (usableHeightNow != usableHeightPrevious)
            {
                var usableHeightSansKeyboard = mChildOfContent.RootView.Height - softButtonsHeight + 20;
                var heightDifference = usableHeightSansKeyboard - usableHeightNow + 20;
                if (heightDifference > (usableHeightSansKeyboard / 4))
                {
                    // keyboard probably just became visible
                    frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference + (softButtonsHeight / 2) + 20;
                }
                else
                {
                    // keyboard probably just became hidden
                    frameLayoutParams.Height = usableHeightSansKeyboard;
                }
                mChildOfContent.RequestLayout();
                usableHeightPrevious = usableHeightNow;
            }
        }

        private int computeUsableHeight()
        {
            var r = new Rect();
            mChildOfContent.GetWindowVisibleDisplayFrame(r);
            return (r.Bottom - r.Top + 20);
        }

        private int getSoftbuttonsbarHeight(IWindowManager windowManager)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var metrics = new DisplayMetrics();
                windowManager.DefaultDisplay.GetMetrics(metrics);
                int usableHeight = metrics.HeightPixels +20;
                windowManager.DefaultDisplay.GetRealMetrics(metrics);
                int realHeight = metrics.HeightPixels + 20;
                if (realHeight > usableHeight)
                    return realHeight - usableHeight;
                else
                    return 0;
            }
            return 0;
        }
    }
}