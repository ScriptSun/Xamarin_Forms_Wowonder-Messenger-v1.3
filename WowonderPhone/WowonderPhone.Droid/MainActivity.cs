using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Com.OneSignal;

using FFImageLoading.Forms;
using Plugin.Media;
using Xamarin.Forms.Platform.Android;
using XLabs.Ioc;
using XLabs.Platform.Device;
using FFImageLoading.Forms.Droid;
using FFImageLoading.Transformations;
using Plugin.Permissions;
using WowonderPhone.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppLinks;
using XLabs.Platform.Services.Geolocation;
using Boolean = System.Boolean;
using Exception = System.Exception;


using ImageCircleRenderer = ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer;

//WOWONDER ANDROID MESSENGER V1.3
//==============================
//This is A sample of xamarin forms chat application 
//This solution is not allowed to be shared on Google play 
//You can get the styles and Xaml codes and use it as you like 
//Devloped by Elin Doughouz
//Follow me on linkded >> https://www.linkedin.com/in/elin-doughouz-1b7458131
//Follow me on Facebook >> https://www.facebook.com/Elindoughous

namespace WowonderPhone.Droid
{
    [Activity(Label = "WowonderPhone", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTop, Theme = "@style/AppTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ClipboardManager AndroidClipboardManager { get; private set; }
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
               
                FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
                FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabs;

                base.OnCreate(bundle);

                if (Settings.BuginAndroidAutoresize)
                {
                    this.Window.AddFlags(WindowManagerFlags.Fullscreen);
                    this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                    Window.SetSoftInputMode(SoftInput.AdjustResize);
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    {
                        // Bug in Android 5+, this is an adequate workaround
                        AndroidBug5497WorkaroundForXamarinAndroid.assistActivity(this, WindowManager);
                    }
                }

                if (Settings.TurnFullScreenOn)
                {
                    this.Window.AddFlags(WindowManagerFlags.Fullscreen);
                    this.Window.AddFlags(WindowManagerFlags.KeepScreenOn);
                }

                if (Settings.TurnSecurityProtocolType3072On)
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                    HttpClient client = new HttpClient(new Xamarin.Android.Net.AndroidClientHandler());
                }

                if (Settings.TurnTrustFailureOn_WebException)
                {
                    //If you are Getting this error >>> System.Net.WebException: Error: TrustFailure /// then Set it to true
                    System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
                    System.Security.Cryptography.AesCryptoServiceProvider b = new System.Security.Cryptography.AesCryptoServiceProvider();
                }

                CachedImageRenderer.Init();
                var ignore = new CircleTransformation();

                var assembliesToInclude = new List<Assembly>()
                {
                    typeof(CachedImage).GetTypeInfo().Assembly,
                    typeof(CachedImageRenderer).GetTypeInfo().Assembly
                };
                AndroidClipboardManager = (ClipboardManager)GetSystemService(ClipboardService);

                var container = new SimpleContainer();
                container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
                container.Register<IGeolocator, Geolocator>();
                Resolver.ResetResolver();
                Resolver.SetResolver(container.GetResolver());
                CrossMedia.Current.Initialize();
                ImageCircleRenderer.Init();

               

                UserDialogs.Init(this);
                global::Xamarin.Forms.Forms.Init(this, bundle);

                AndroidAppLinks.Init(this);

                var data = Intent?.Data?.EncodedAuthority.ToString();

                //Uri data = getIntent().getData();
                //String scheme = data.getScheme(); // "http"
                //String host = data.getHost(); // "twitter.com"
                //List < String > params = data.getPathSegments();
                //String first = params.get(0); // "status"
                //String second = params.get(1); // "1234"

               

                LoadApplication(new App());
               
            }
            catch (Exception ex)
            {
               
            }

        }
        protected static Boolean isVisible = false;



        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            //DeviceOrientationLocator.NotifyOrientationChanged();
        }


        protected override void OnNewIntent(Intent intent)
        {
            try
            {
                if (intent.Extras.ContainsKey("username") && this.HasWindowFocus)
                {
                    var Username = intent.Extras.Get("username").ToString();
                    var UserID = intent.Extras.Get("userid").ToString();
                    App.Current.MainPage.Navigation.PushAsync(new ChatWindow(Username, UserID,null));

                    try
                    {
                        var context = Forms.Context;
                        var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                        int useridNumber = Int32.Parse(UserID);
                        notificationManager.Cancel(useridNumber);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {
                return;
            }
            base.OnNewIntent(intent);
        }

      
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
