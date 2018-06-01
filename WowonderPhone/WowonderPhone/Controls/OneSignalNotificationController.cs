using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using Xamarin.Forms;


//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on Linkedin >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous


namespace WowonderPhone.Controls
{
    public class OneSignalNotificationController 
    {
        public static void RegisterNotificationDevice()
        {
           
          
           //OneSignal.Current.PromptLocation();
           OneSignal.Current.StartInit(Settings.Onesignal_APP_ID).InFocusDisplaying(OSInFocusDisplayOption.None)
                     .HandleNotificationReceived(HandleNotificationReceived)
                     .HandleNotificationOpened(HandleNotificationOpened).EndInit();
            OneSignal.Current.IdsAvailable(IdsAvailable);
            OneSignal.Current.RegisterForPushNotifications();
            

        }

        private static void IdsAvailable(string userID, string pushToken)
        {
            Settings.Device_ID = userID;
        }

        private static void HandleNotificationReceived(OSNotification notification)
        {
            OSNotificationPayload payload = notification.payload;
           
        }

        private static  void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            OSNotificationPayload payload = result.notification.payload;
            Dictionary<string, object> additionalData = payload.additionalData;
            string message = payload.body;
            string actionID = result.action.actionID;
            
            if (additionalData != null)
            {
                if (additionalData.ContainsKey("discount"))
                {
                   
                    // Take user to your store.
                }
            }
            if (actionID != null)
            {
                // actionSelected equals the id on the button the user pressed.
                // actionSelected will equal "__DEFAULT__" when the notification itself was tapped when buttons were present. 
            }
        }
    }
}
