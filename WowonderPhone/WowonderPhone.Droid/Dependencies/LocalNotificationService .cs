using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using WowonderPhone.Dependencies;
using WowonderPhone.Droid.Dependencies;
using Xamarin.Forms;


[assembly: Dependency(typeof(LocalNotificationService))]
namespace WowonderPhone.Droid.Dependencies
{
    public class LocalNotificationService : Java.Lang.Object, ILocalNotificationService
    {

        public void CreateLocalNotification(string text, string username, string image, string userid, string Kindoff)
        {
            try
            {
                Bundle valuesForActivity = new Bundle();
                valuesForActivity.PutString("userid", userid);
                valuesForActivity.PutString("username", username);

                var context = Forms.Context;
                var resultIntent = new Intent(context, typeof(MainActivity));
                resultIntent.PutExtras(valuesForActivity);

                PendingIntent resultPendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

                //Set style of the application
                //AddAction(Resource.Drawable.Forward, "Dismiss", resultPendingIntent).AddAction(Resource.Drawable.PaperPlane, "Reply", resultPendingIntent)

                var ss = getBitmap(image);
                if (Kindoff == "Text")
                {
                    var newColor = new Android.Graphics.Color(46, 146, 44);
                    Notification.Builder builder = new Notification.Builder(context)
                        .SetAutoCancel(true)
                        .SetLargeIcon(ss).SetVibrate(new long[0])
                        .SetSmallIcon(Resource.Drawable.icon).AddAction(Resource.Drawable.Privacy, "View", resultPendingIntent)
                        .SetContentTitle(username).SetPriority(5)
                        .SetContentText(String.Format(text)).SetTicker(text)
                        .SetContentIntent(resultPendingIntent);

                    Notification notification = builder.Build();
                    notification.Color = Android.Graphics.Color.ParseColor(Settings.MainColor);


                    if (Settings.NotificationVibrate)
                    {
                        notification.Defaults |= NotificationDefaults.Vibrate;
                    }
                    if (Settings.NotificationSound)
                    {
                        notification.Defaults |= NotificationDefaults.Sound;
                    }


                    notification.Flags = NotificationFlags.ShowLights;
                    notification.LedARGB = GetLedColor();
                    notification.LedOnMS = 500;
                    notification.LedOffMS = 1000;




                    var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                    int useridNumber = Int32.Parse(userid);
                    notificationManager.Notify(useridNumber, notification);

                }

                ///////// NEXT Update >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //else if (Kindoff == "LongText")
                //{

                //    Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

                //    textStyle.BigText(text);
                //    textStyle.SetSummaryText("The summary text goes here.");
                //    Notification.Builder builder = new Notification.Builder(context)

                //   .SetAutoCancel(true)
                //   .SetLargeIcon(ss)
                //   .SetStyle(textStyle)
                //   .SetSmallIcon(Resource.Drawable.default_profile_6_400x400)
                //   .SetContentTitle(username)
                //   .SetTicker(text).SetPriority(2)

                //   .SetContentIntent(resultPendingIntent);

                //    var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                //    int useridNumber = Int32.Parse(userid);
                //    notificationManager.Notify(useridNumber, builder.Build());
                //    NOTIFICATION_ID++;
                //}>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


            }
            catch (Exception)
            {

            }

        }


        public Android.Graphics.Color GetLedColor()
        {
            if (Settings.NotificationLedColor == Settings.Label_Default)
            {
                return Android.Graphics.Color.ParseColor(Settings.MainColor);
            }

            else if (Settings.NotificationLedColor == "#00FF00")
            {
                return Android.Graphics.Color.Green;
            }
            else if (Settings.NotificationLedColor == "#0000FF")
            {
                return Android.Graphics.Color.Blue;
            }
            else if (Settings.NotificationLedColor == "#FF00FF")
            {
                return Android.Graphics.Color.Magenta;
            }
            else if (Settings.NotificationLedColor == "#ffa500")
            {
                return Android.Graphics.Color.Orange;
            }
            else if (Settings.NotificationLedColor == "#C0C0C0")
            {
                return Android.Graphics.Color.Silver;
            }
            else if (Settings.NotificationLedColor == "#FFFF00")
            {
                return Android.Graphics.Color.Yellow;
            }
            else if (Settings.NotificationLedColor == "#800000")
            {
                return Android.Graphics.Color.Maroon;
            }
            else if (Settings.NotificationLedColor == "#800080")
            {
                return Android.Graphics.Color.Purple;
            }
            else if (Settings.NotificationLedColor == "#FF0000")
            {
                return Android.Graphics.Color.Red;
            }
            else
            {
                return Android.Graphics.Color.ParseColor(Settings.MainColor);
            }
        }
        public void CloseNotification(string Id)
        {
            try
            {
                var context = Forms.Context;
                var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                int useridNumber = Int32.Parse(Id);
                notificationManager.Cancel(useridNumber);
            }
            catch (Exception)
            {

            }
        }
        public static Bitmap getBitmap(String photoPath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InPreferredConfig = Bitmap.Config.Argb8888;
            //options.InSampleSize = 6;
            return BitmapFactory.DecodeFile(photoPath, options);
        }
    }
}