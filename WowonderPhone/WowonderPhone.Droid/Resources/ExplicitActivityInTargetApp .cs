using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Acr.UserDialogs;
using Xamarin.Forms.Platform.Android.AppLinks;
using Xamarin.Forms;

namespace WowonderPhone.Droid.Resources
{
    [Activity(Label = "Explicit in Target", Exported = true, Name = "com.wowonder.messenger.ExplicitActivityInTargetApp")]
    public class ExplicitActivityInTargetApp : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabs;

            base.OnCreate(savedInstanceState);


            UserDialogs.Init(this);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            AndroidAppLinks.Init(this);

            var data = Intent?.Data?.EncodedAuthority.ToString();

            //Uri data = getIntent().getData();
            //String scheme = data.getScheme(); // "http"
            //String host = data.getHost(); // "twitter.com"
            //List < String > params = data.getPathSegments();
            //String first = params.get(0); // "status"
            //String second = params.get(1); // "1234"


            App.Navigation.PushModalAsync(new WowonderPhone.MainPage());
           
        }
    }
}