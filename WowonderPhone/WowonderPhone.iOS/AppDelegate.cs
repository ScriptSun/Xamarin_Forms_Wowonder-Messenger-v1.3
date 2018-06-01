using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Foundation;
using Plugin.Media;
using UIKit;
using UserNotifications;
using UXDivers.Artina.Grial;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Geolocation;

namespace WowonderPhone.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        #region Computed Properties
        public override UIWindow Window { get; set; }
		//public SoundRecord AudioManager { get; set; } = new SoundRecord();
		#endregion

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
			var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

			var resolverContainer = new SimpleContainer();
			resolverContainer.Register<IDevice>(t => AppleDevice.CurrentDevice).Register<IDisplay>(t => t.Resolve<IDevice>().Display)
							 .Register<IDependencyContainer>(t => resolverContainer);
			 Resolver.ResetResolver();
			 Resolver.SetResolver(resolverContainer.GetResolver());
			
           var workaround = typeof(UXDivers.Artina.Shared.CircleImage);
			
            global::Xamarin.Forms.Forms.Init();
			ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init();

            Appearance.Configure();
            FFImageLoading.Forms.Touch.CachedImageRenderer.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
		#region Override Methods
      
		//public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		//{
		//	// show an alert
		//	UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
		//	okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

		//	Window.RootViewController.PresentViewController(okayAlertController, true, null);

		//	// reset our badge
		//	UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		//}

		public override void OnResignActivation(UIApplication application)
		{

		}

		public override void DidEnterBackground(UIApplication application)
		{
			//AudioManager.SuspendBackgroundMusic();
			//AudioManager.DeactivateAudioSession();
		}

		public override void WillEnterForeground(UIApplication application)
		{
			//AudioManager.ReactivateAudioSession();
			//AudioManager.RestartBackgroundMusic();
		}

		public override void OnActivated(UIApplication application)
		{

		}

		public override void WillTerminate(UIApplication application)
		{
			//AudioManager.StopBackgroundMusic();
			//AudioManager.DeactivateAudioSession();
		}
		#endregion
	}

	

	public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
	{
		#region Constructors
		public UserNotificationCenterDelegate()
		{
		}
		#endregion

		#region Override Methods
		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			// Do something with the notification
			Console.WriteLine("Active Notification: {0}", notification);

			completionHandler(UNNotificationPresentationOptions.Alert);
		}

		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			// Take action based on Action ID
			switch (response.ActionIdentifier)
			{
				case "reply":
					// Do something
					break;
				default:
					// Take action based on identifier
					switch (response.ActionIdentifier)
					{
						case "":
                    // Handle default
                  
                    break;
						case "dfd":
                    // Handle dismiss
               
                    break;
					}
					break;
			}

			// Inform caller it has been handled
			completionHandler();
		}
		#endregion
	}
}
