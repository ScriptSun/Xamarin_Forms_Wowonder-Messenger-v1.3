using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Foundation;
using SystemConfiguration;
using UIKit;
using UserNotifications;
using WowonderPhone.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(WowonderPhone.iOS.Dependencies.LocalNotificationService))]
namespace WowonderPhone.iOS.Dependencies
{
    public class LocalNotificationService : ILocalNotificationService
    {
        public void CreateLocalNotification(string text, string username, string image, string userid, string Kindoff)
        {
            try
            {
                var content = new UNMutableNotificationContent();
				content.Subtitle = username;
				content.Body = text;
				content.Badge = 5;

			
				var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
				var requestID = userid;
				var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);

				//var actionID = "reply";
				//var title = "Reply";
				//var action = UNNotificationAction.FromIdentifier(actionID, title, UNNotificationActionOptions.None);
				//var actions = new UNNotificationAction[] { action };
				//var intentIDs = new string[] { };
				//var categoryOptions = new UNNotificationCategoryOptions[] { };
				//var category = UNNotificationCategory.FromIdentifier(userid, actions, intentIDs, UNNotificationCategoryOptions.None);
				//var categories = new UNNotificationCategory[] { category };
				//UNUserNotificationCenter.Current.SetNotificationCategories(new NSSet<UNNotificationCategory>(categories));

				UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
				{
					if (err != null)
					{
						// Do something with error...
					}
				});



            }
            catch (Exception)
            {

            }

        }

        public void CloseNotification(string Id)
        {
            try
            {
               var requests = new string[] { Id };
				UNUserNotificationCenter.Current.RemoveDeliveredNotifications(requests);
            }
            catch (Exception)
            {

            }
        }

    }
}
