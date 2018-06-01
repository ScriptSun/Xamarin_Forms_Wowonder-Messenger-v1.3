using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using WowonderPhone.Controls;
using WowonderPhone.Dependencies;
using WowonderPhone.Languish;
using WowonderPhone.Pages;
using WowonderPhone.Pages.Register_pages;
using WowonderPhone.SQLite;
using Xamarin.Forms;
using XLabs.Forms.Services;

//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on linkded >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous


namespace WowonderPhone
{
    public partial class App : Application
    {

        public static INavigation Navigation { get; set; }

        public App()
        {
			
           L10n.SetLocale();
           var netLanguage = DependencyService.Get<ILocale>().GetCurrent();
           AppResources.Culture = new CultureInfo(netLanguage);

            SQL_Entity.Connect();


			
                //Data.ClearLoginCredentialsList();
                var CredentialStatus = SQL_Commander.GetLoginCredentialsStatus();


                InitializeComponent();

              
                if (CredentialStatus == "Active")
                {
                    var Credential = SQL_Commander.GetLoginCredentials("Active");
                    Settings.Session = Credential.Session;
                    Settings.User_id = Credential.UserID;
                    Settings.Username = Credential.Username;
                    Settings.Onesignal_APP_ID = Credential.Onesignal_APP_ID;
                    if (Credential.NotificationLedColor != "")
                    {

                        Settings.NotificationVibrate = Credential.NotificationVibrate;
                        Settings.NotificationSound = Credential.NotificationSound;
                        Settings.NotificationPopup = Credential.NotificationPopup;
                        Settings.NotificationLedColor = Credential.NotificationLedColor;
                        Settings.NotificationLedColorName = Credential.NotificationLedColor;

                    }
                    else
                    {
                        Credential.NotificationVibrate = true;
                        Credential.NotificationLedColor = Settings.MainColor;
                        Credential.NotificationLedColorName = AppResources.Label_Led_Color;
                        Credential.NotificationSound = true;
                        Credential.NotificationPopup = true;
                        SQL_Commander.UpdateLoginCredentials(Credential);
                        Settings.NotificationVibrate = true;
                        Settings.NotificationSound = true;
                        Settings.NotificationPopup = true;
                        Settings.NotificationLedColor = Settings.MainColor;
                        Settings.NotificationLedColorName = AppResources.Label_Led_Color;
                    }


                    //Start Onesignal
                    OneSignalNotificationController.RegisterNotificationDevice();
                    var navigationPage = new NavigationPage(new MasterMain()) { };
                    navigationPage.BarBackgroundColor = Color.FromHex(Settings.MainPage_HeaderBackround_Color);
                    navigationPage.BarTextColor = Color.FromHex(Settings.MainPage_HeaderText_Color);
                    navigationPage.Title = Settings.MainPage_HeaderTextLabel;
                    navigationPage.Padding = new Thickness(0, 0,0,0);
                    MainPage = navigationPage;


                }
                else
                {
                    if (CredentialStatus == "Registered")
                    {
                        var Credential = SQL_Commander.GetLoginCredentials("Registered");
                        Settings.Session = Credential.Session;
                        Settings.User_id = Credential.UserID;
                        Settings.Username = Credential.Username;
                        MainPage = new NavigationPage(new UploudPicPage());

                    }
                    else
                    {
                      
                        MainPage = new NavigationPage(new WelcomePage());
                        
                    }

                }
            
        }
        public static void GetMainPage()
        {
            try
            {
                var navigationPage = new NavigationPage(new MasterMain()) {};
                navigationPage.BarBackgroundColor = Color.FromHex(Settings.MainPage_HeaderBackround_Color);
                navigationPage.BarTextColor = Color.FromHex(Settings.MainPage_HeaderText_Color);
                navigationPage.Title = Settings.MainPage_HeaderTextLabel;
                navigationPage.Padding = new Thickness(0, 0, 0, 0);
                App.Current.MainPage = navigationPage;
            }
            catch
            {
                App.Current.MainPage = new MasterMain();
            }

        }

        
        public static void GetRegisterPage()
        {
           
            App.Current.MainPage = new NavigationPage(new RegisterFriends());
        }

        public static void GetLoginPage()
        {
            App.Current.MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            var data = uri.ToString().ToLowerInvariant();
            UserDialogs.Instance.Alert(data);
            if (!data.Contains("/Wowonder/"))
                return;

            var id = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1);
            App.Current.MainPage = new NavigationPage(new ChatWindow("ali doughouz", "454545",null)); ;
         
            //Navigate based on id here.

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
