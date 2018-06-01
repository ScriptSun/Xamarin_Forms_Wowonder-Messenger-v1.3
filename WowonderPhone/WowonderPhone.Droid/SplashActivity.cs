using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    [Activity(NoHistory = true, Theme = "@style/Theme.Splash", MainLauncher = true)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView }, DataScheme = "wowonder", Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    [IntentFilter(new[] { Android.Content.Intent.ActionView }, Categories = new[]{ Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable}, DataScheme = "http",DataPathPrefix = "/Wowonder/",DataHost = "demo.Wowonder.com")]
    public class SplashActivity : Activity
    {
        private Bundle savedInstanceState;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(MainActivity));

          

            //OnNewIntent(Intent);
        }

       
    }
}