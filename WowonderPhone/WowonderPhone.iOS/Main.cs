using System;
using System.Collections.Generic;
using System.Linq;
using Acr.UserDialogs;
using Foundation;
using UIKit;

namespace WowonderPhone.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
			try
			{
				// if you want to use a different Application Delegate class from "AppDelegate"
				// you can specify it here.
				UIApplication.Main(args, null, "AppDelegate");
				Plugin.Media.CrossMedia.Current.Initialize();

			}
            catch(Exception ex) 
            {
                var e = ex.ToString();
            }
        }
    }
}
